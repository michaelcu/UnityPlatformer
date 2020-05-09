using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 5f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;

    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            // Frog Reaches Left Cap
            if (transform.position.x > leftCap)
            {

                // Make sure sprite faces right direct if not then face right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                // Jump only when toucching the ground
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping",true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            // Frog Reaches Right Cap
            if (transform.position.x < rightCap)
            {

                // Make sure sprite faces right direct if not then face right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

}
