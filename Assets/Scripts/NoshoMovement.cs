using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoshoMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public int jumpsAmount;
    int jumpsLeft;
    private BoxCollider2D boxCollider;
    public LayerMask GroundLayer;

    bool isGrounded;
    bool isGrounded2;

    private bool isInsidePumpkinfoolCollider = false;

    float moveInput;
    Rigidbody2D rb2d;
    float scaleX;

    float groundCheckDistance = 0.02f;

    public Animator animator;
    public ParticleSystem collectEffect;
    public AudioSource src;
    public AudioClip munchSfx;

    public ChatBubble cb;

    public AudioSource footstepSrc;
    public AudioClip stepSfx;
    public AudioClip jumpSfx;
    bool isFootstepsPlaying = false;

    public bool isDead = false;
    public AudioClip deathSfx;


    public LadderMovement ladderMovement;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            isGrounded2 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, groundCheckDistance, GroundLayer);

            animator.SetBool("isGrounded", isGrounded2);

            moveInput = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("speed", Mathf.Abs(moveInput * moveSpeed));
            Jump();


            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move();


            if (Mathf.Abs(moveInput) == 1 && isGrounded2)
            {
                if (!isFootstepsPlaying)
                {
                    footstepSrc.clip = stepSfx;
                    footstepSrc.loop = true;
                    footstepSrc.Play();
                    isFootstepsPlaying = true;
                }
            }
            else
            {
                if (isFootstepsPlaying)
                {
                    if (footstepSrc.clip == stepSfx)
                        footstepSrc.Stop();
                    footstepSrc.loop = false;
                    isFootstepsPlaying = false;
                }
            }
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
        
        if (Input.GetKeyDown(KeyCode.W) && !ladderMovement.isClimbing)
        {
            CheckIfGrounded();
            if (jumpsLeft > 0)
            {
                footstepSrc.Stop();
                footstepSrc.clip = jumpSfx;
                footstepSrc.Play();
                animator.SetTrigger("isJumping");
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
        if (isInsidePumpkinfoolCollider)
        {
            cb.CheckCandy();
        }
    }

    void Death()
    {
        isDead = true;
        rb2d.velocity = new Vector2(0, 0);
        animator.SetTrigger("isDead");
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
            Death();
            collision.gameObject.GetComponent<Spike>().bloodParticles();
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
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
    }


}