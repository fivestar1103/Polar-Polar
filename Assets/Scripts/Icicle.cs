using UnityEngine;

public class Icicle : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float triggerDistance = 5f;
    public float multiplyCoefficient = 3f;
    public LayerMask groundLayer;
    public Sprite northPoleSprite;
    public Sprite southPoleSprite;
    public float homingIntensity = 0.1f;
    public GameObject magneticEffectPrefab;

    
    private bool isFalling = false;
    private bool isHoming = true;
    private GameObject _player;
    private Rigidbody2D rb;
    private bool isPoleNorth;
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
        rb.isKinematic = true;

        isPoleNorth = Random.value > 0.5f;
        spriteRenderer.sprite = isPoleNorth ? northPoleSprite : southPoleSprite;
    }

    void Update()
    {
        if (!player) return;

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
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToPlayer.x * homingIntensity, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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

        if (horizontalDistance <= triggerDistance)
        {
            Instantiate(magneticEffectPrefab, transform.position, Quaternion.identity);

            float magneticForceMultiplied = player.GetComponent<PenguinController>().magneticForce * multiplyCoefficient;
            if (isPoleNorth == isNorthPole)
            {
                rb.AddForce(-directionToPlayer.normalized * magneticForceMultiplied, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(directionToPlayer.normalized * magneticForceMultiplied, ForceMode2D.Impulse);
            }
            isHoming = false;
        }
    }
}
