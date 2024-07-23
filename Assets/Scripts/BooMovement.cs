using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public int jumpsAmount;
    int jumpsLeft;
    private BoxCollider2D boxCollider;
    public LayerMask GroundLayer;

    bool isGrounded;
    private bool isInsidePumpkinfoolCollider = false;

    float moveInput;
    Rigidbody2D rb2d;
    float scaleX;

    float groundCheckDistance = 0.05f;
    // Start is called before the first frame update

    public Animator animator;
    public ParticleSystem collectEffect;
    public AudioSource src;
    public AudioClip munchSfx;

    public ChatBubble cb;

    public bool isInsideGraveCollider = false;
    public GraveManager graveManager;
    public bool isDisappeared = false;


    private Grave interactingWithGrave;
    public AudioClip graveSfx;

    public AudioSource footstepSrc;
    public AudioClip jumpSfx;

    public bool isDead = false;
    public AudioClip deathSfx;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Interact();
            }


            if (isDisappeared)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    graveManager.SwitchToNextGrave();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    graveManager.SwitchToPreviousGrave();
                }
            }
            else
            {
                moveInput = Input.GetAxisRaw("Vertical");
                Jump();

                animator.SetFloat("speed", Mathf.Abs(moveInput * moveSpeed));
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
        }
    }

    public void Move()
    {
        Flip();
        rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y);
    }

    public void Flip()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        if (moveInput < 0)
        {
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CheckIfGrounded();
            if (jumpsLeft > 0)
            {
                footstepSrc.Stop();
                footstepSrc.clip = jumpSfx;
                footstepSrc.Play();
                animator.SetTrigger("isJumping 0");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                jumpsLeft--;
            }

        }

    }

    public void CheckIfGrounded()
    {
        isGrounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundCheckDistance, GroundLayer);

        //Debug.DrawRay(boxCollider.bounds.center, Vector2.down * groundCheckDistance, isGrounded?Color.green:Color.red);

        ResetJumps();
    }

    public void ResetJumps()
    {
        if (isGrounded)
        {
            
            jumpsLeft = jumpsAmount;// jumpsAmount =2;
        }
    }

    public void Interact()
    {
        
        if (isDisappeared)
        {
            Debug.Log("Trying!");
            graveManager.TeleportPlayerToSelectedGrave();

            src.clip = graveSfx;
            src.Play();

            GetComponent<Renderer>().enabled = true;

            animator.SetTrigger("isUngraving");

            isDisappeared = false;

        }
        else
        {
            if (isInsideGraveCollider)
            {

                animator.SetTrigger("isGraving");

                Invoke("DelayedDissapear", 0.62f);

                
            }
        }

        if (isInsidePumpkinfoolCollider && !isDisappeared)
        {
            cb.CheckCandy();
        }

    }

    void DelayedDissapear()
    {
        interactingWithGrave.Particles();
        src.clip = graveSfx;
        src.Play();

        GetComponent<Renderer>().enabled = false;

        isDisappeared = true;

        graveManager.SetFirstSelectedGrave(interactingWithGrave);


    }

    void Death()
    {
        isDead = true;
        animator.SetTrigger("isDead");
        rb2d.velocity = new Vector2(0, 0);
        footstepSrc.Stop();
        src.Stop();
        src.loop = false;
        src.clip = deathSfx;
        src.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            collision.gameObject.GetComponent<Spike>().bloodParticles();
            Death();
        }
    }

    public void SetGraveTeleported(Grave grave)
    {
        isInsideGraveCollider = true;
        interactingWithGrave = grave;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grave"))
        {
            isInsideGraveCollider = true;
            interactingWithGrave = other.GetComponent<Grave>();
        }

        if (other.CompareTag("pfool"))
        {
            isInsidePumpkinfoolCollider = true;
        }

        if (other.CompareTag("Candy"))
        {
            CandyManager candyManager = GameObject.Find("CandyManager").GetComponent<CandyManager>();
            candyManager.CollectCandy();

            src.clip = munchSfx;
            src.Play();
            collectEffect.textureSheetAnimation.RemoveSprite(0);
            collectEffect.textureSheetAnimation.AddSprite(other.GetComponent<SpriteRenderer>().sprite);
            if (collectEffect != null)
            {
                collectEffect.Play();
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("pfool"))
        {
            isInsidePumpkinfoolCollider = false;
        }
        if (collision.CompareTag("Grave"))
        {
            isInsideGraveCollider = false;
        }
    }


}