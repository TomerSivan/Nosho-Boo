using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    public float vertical;
    private float speed = 2.5f;
    private bool isLadder;
    public bool isClimbing;
    private bool wasClimbing;

    public Animator animator;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxis("UpDown");
        wasClimbing = isClimbing;

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        // Check if the climbing state has changed
        if (isClimbing != wasClimbing)
        {
            if (animator)
            {
                if (isClimbing)
                {
                    animator.SetTrigger("isClimbing");
                }
                else
                {
                    animator.SetTrigger("isNoClimbing");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0.09f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            if (animator)
            {
                animator.SetTrigger("isNoClimbing");
            }
        }
    }
}