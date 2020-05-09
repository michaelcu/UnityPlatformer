using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    // OnStart Variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;



    // States
    private enum PlayerState { idle, running, jumping, falling, hurt }
    private PlayerState state = PlayerState.idle;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;


    // Inspector Variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpforce = 12f;
    [SerializeField] private float hurtforce = 10f;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource hurt;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        footstep = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if(state != PlayerState.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); // Sets states based on player's vertical and horizontal velocity
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            Debug.Log("Collided with Cherry");
            Cherry();
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == PlayerState.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = PlayerState.hurt;
                Hurt();
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    // Damaged from right and move left
                    rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                }
                else
                {
                    // Damaged from left and move right
                    rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        // Player's Horizontal Axis
        float hdirection = Input.GetAxis("Horizontal");


        // Move Left
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // Move Right
        if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }

    }
    
    private void Jump()
    {
        // Player Jump
        rb.velocity = new Vector2(rb.velocity.x,jumpforce);
        state = PlayerState.jumping;
    }

    private void AnimationState()
    {

        // Player is Jumping
        if(state == PlayerState.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = PlayerState.falling;

            }
        }


        // Player is falling
        else if(state == PlayerState.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = PlayerState.idle;
            }
        }

        else if (state == PlayerState.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = PlayerState.idle;
            }
        }


        // Player is moving
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = PlayerState.running;
        }
        
        // Player is still
        else
        {
            state = PlayerState.idle;
        }

    }

    private void Footstep()
    {
        footstep.Play(); // Plays Footstep Sound Effect
    }

    private void Cherry() 
    {
        cherry.Play(); // Plays Cherry Sound Effect
    }

    private void Hurt()
    {
        hurt.Play(); // Plays Hurt Sound Effect
    }
}
