using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator animator;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 15f;

    private enum MovementState { idle, run, jump, fall};
    private bool canDoubleJump;

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        //if (Input.GetButtonDown("Jump") && IsGrounded() == true) // Single Jump
        //{
        //    jumpSoundEffect.Play();
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        // Double Jump
        if (IsGrounded())
        {
            canDoubleJump = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else
            {
                if (canDoubleJump)
                {
                    jumpSoundEffect.Play();
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canDoubleJump= false;
                }
            }
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jump;   
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.fall;
        }
        animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
