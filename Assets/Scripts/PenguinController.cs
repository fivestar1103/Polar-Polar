using UnityEngine;

public class PenguinController : MonoBehaviour
{
    public float speed = 5f;
    public float slideSpeed = 8f;
    public float jumpForce = 10f;
    public float maxWalkableAngle = 0f; 
    public float magneticForce = 10f;
    private bool isNorthPoleActive = true;
    private bool isOnMatchingPole;
    
    public LayerMask groundLayer;
    public Vector2 slideColliderSize = new Vector2(2.55f, 1.86f);
    public Vector2 originalColliderSize;
    public GameObject hitEffectPrefab;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private Animator animator;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isSliding = false;
    
    private const string WALK_PARAM = "IsWalking";
    private const string JUMP_PARAM = "IsJumping";
    private const string SLIDE_PARAM = "IsSliding";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        originalColliderSize = coll.size;
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(coll, groundLayer);
        float move = Input.GetAxis("Horizontal");

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Q)) && isGrounded)
        {
            StartSlide();
        }

        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Q)) && isSliding)
        {
            StopSlide();
        }

        if (Mathf.Abs(move) > 0f && !isSliding)
        {
            animator.SetBool(WALK_PARAM, true);
            Flip(move);
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }
        else if (!isSliding)
        {
            animator.SetBool(WALK_PARAM, false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool(JUMP_PARAM, true);
        }
        else if (isGrounded)
        {
            animator.SetBool(JUMP_PARAM, false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isNorthPoleActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isNorthPoleActive = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            string attackAnimation = isNorthPoleActive ? "penguin_attack_N" : "penguin_attack_S";
            animator.Play(attackAnimation);

            if (isOnMatchingPole)
            {
                ApplyMagneticRepulsion();
            }
        }
    }

    private void StartSlide()
    {
        isSliding = true;
        animator.SetBool(SLIDE_PARAM, true);
        rb.velocity = new Vector2(isFacingRight ? slideSpeed : -slideSpeed, rb.velocity.y);
        coll.size = slideColliderSize; 
        coll.direction = CapsuleDirection2D.Horizontal;
    }

    private void StopSlide()
    {
        isSliding = false;
        animator.SetBool(SLIDE_PARAM, false);
        coll.size = originalColliderSize;
        coll.direction = CapsuleDirection2D.Vertical;
    }

    private void UseMagneticPowers()
    {
        if (isNorthPoleActive)
        {
            animator.Play("penguin_attack_N");
        }
        else
        {
            animator.Play("penguin_attack_S");
        }
    }

    private bool CanWalkOnSlope()
    { 
        // float colliderBottomHeight = coll.bounds.extents.y;
        // Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y-colliderBottomHeight-0.5f);
        // Debug.DrawRay(rayOrigin, Vector2.down*5f, Color.red);
        // RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f);
        // if (hit)
        // {
        //     Vector2 normal = hit.normal;
        //     float slopeAngle = Vector2.Angle(normal, Vector2.up);
        //     // Debug.Log(slopeAngle + ", " + hit.collider.name);
        //
        //     if (slopeAngle <= maxWalkableAngle) return true;
        // }
        // return false;
        return true;
    }

    private void Flip(float move)
    {
        if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("sharp"))
        {
            GameManager.instance.RespawnPenguin();
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        else if (other.CompareTag("water"))
        {
            GameManager.instance.RespawnPenguin();
        }
        else if (other.CompareTag("candy"))
        {
            GameManager.instance.PenguinPickedCandy();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Finish"))
        {
            GameManager.instance.ClearLevel();
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        isOnMatchingPole = (other.CompareTag("NorthPole") && isNorthPoleActive) || 
                           (other.CompareTag("SouthPole") && !isNorthPoleActive);
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NorthPole") || other.CompareTag("SouthPole"))
        {
            isOnMatchingPole = false;
        }
    }

    private void ApplyMagneticRepulsion()
    {
        rb.AddForce(Vector2.up * magneticForce, ForceMode2D.Impulse);
        isOnMatchingPole = false;
    }
}
