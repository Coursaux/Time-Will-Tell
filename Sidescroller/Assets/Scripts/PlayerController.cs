using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform groundCheck;

    //Knockback
    private bool invulnerable = false;
    public const float knockBackThrust = 150;
    private float KnockbackStart = -10f;
    private float knockbackLength = 0.7f;

    //Movement
    public float speed;
    public float jump;
    float moveVelocity;

    //Grounded Vars
    bool grounded = false;



    void Update()
    {
        // change colour back after getting hurt
        if (KnockbackStart + knockbackLength < Time.time && invulnerable)
        {
            invulnerable = false;
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }

        // check for movement after being knocked back
        if (!invulnerable)
        {
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

            //Jumping
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
            {
                if (grounded)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
                }
            }

            moveVelocity = 0;

            //Left Right Movement
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                moveVelocity = -speed * Time.deltaTime;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                moveVelocity = speed * Time.deltaTime;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            // calculating which way to knockback
            float dif = (col.transform.position.x - transform.position.x);
            KnockBack(dif, true);
        }
    }

    public void KnockBack(float direction, bool damaged, float thrust = knockBackThrust)
    {
        if (!invulnerable)
        {
            if (damaged)
            {
                FlashDamaged();
            }
            KnockbackStart = Time.time;
            invulnerable = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            rb.AddForce(transform.up * thrust);
            if (direction > 0)
                rb.AddForce(transform.right * -thrust);
            else if (direction < 0)
                rb.AddForce(transform.right * thrust);
        }
    }

    private void FlashDamaged()
    {
        Debug.Log("wtf");
        GetComponent<SpriteRenderer>().color = new Color32(219, 92, 92, 255);
    }
}