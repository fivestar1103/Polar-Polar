using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float triggerDistance = 5f;
    public float multiplyCoefficient = 3f;
    public LayerMask groundLayer;
    public Sprite northPoleSprite;
    public Sprite southPoleSprite;
    public float homingIntensity = 0.1f; // Adjust this to control how strongly the icicle homes in on the player
    public GameObject magneticEffectPrefab;

    
    private bool isFalling = false;
    private bool isHoming = true; // New flag to control homing
    private GameObject _player;
    private Rigidbody2D rb;
    private bool isPoleNorth; // True for North, false for negative
    private SpriteRenderer spriteRenderer;
    
    private GameObject player
    {
        get
        {
            if (!_player) _player = GameObject.FindGameObjectWithTag("Player");
            return _player;
        }
    }
    
    void OnEnable()
    {
        PenguinController.OnMagneticPowerUsed += ApplyMagneticForce;
    }

    void OnDisable()
    {
        PenguinController.OnMagneticPowerUsed -= ApplyMagneticForce;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.isKinematic = true; // Prevents the icicle from falling due to gravity until triggered

        // Randomly assign pole and corresponding sprite
        isPoleNorth = Random.value > 0.5f;
        spriteRenderer.sprite = isPoleNorth ? northPoleSprite : southPoleSprite;
    }

    void Update()
    {
        if (!player) return; // Check if player is still null

        // Only calculate horizontal distance to trigger fall
        float horizontalDistance = Mathf.Abs(transform.position.x - player.transform.position.x);
        if (!isFalling && horizontalDistance <= triggerDistance)
        {
            StartFalling();
        }

        if (isFalling && isHoming)
        {
            AdjustTrajectoryTowardsPlayer();
        }
    }

    void StartFalling()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector2(0, -fallSpeed);
        isFalling = true;
    }
    
    void AdjustTrajectoryTowardsPlayer()
    {
        // Calculate direction towards player
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        // Apply a horizontal force towards the player
        rb.velocity = new Vector2(directionToPlayer.x * homingIntensity, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy icicle when it hits the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            PenguinController penguinController = player.GetComponent<PenguinController>();
            if (penguinController != null)
            {
                // Check if the penguin is using magnetic powers
                if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Equals))
                {
                    ApplyMagneticForce(penguinController.isNorthPoleActive);
                }
            }
        }
    }

    private void ApplyMagneticForce(bool isNorthPole)
    {
        if (!player || !isFalling) return;

        Vector2 directionToPlayer = player.transform.position - transform.position;
        float horizontalDistance = Mathf.Abs(directionToPlayer.x);

        // Check if within a certain range to apply magnetic force
        if (horizontalDistance <= triggerDistance)
        {
            Instantiate(magneticEffectPrefab, transform.position, Quaternion.identity);

            float magneticForceMultiplied = player.GetComponent<PenguinController>().magneticForce * multiplyCoefficient;
            if (isPoleNorth == isNorthPole)
            {
                // Repel
                rb.AddForce(-directionToPlayer.normalized * magneticForceMultiplied, ForceMode2D.Impulse);
            }
            else
            {
                // Attract
                rb.AddForce(directionToPlayer.normalized * magneticForceMultiplied, ForceMode2D.Impulse);
            }
            isHoming = false; // Stop homing after being repelled
        }
    }
}
