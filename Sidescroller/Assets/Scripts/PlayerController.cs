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

    // ability
    [HideInInspector]
    public float timeAmount = 0;

    void Update()
    {
        if (!GetComponent<HealthManager>().dead)
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
                if (Input.GetButtonDown("Jump"))
                {
                    if (grounded)
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
                    }
                }

                moveVelocity = 0;

                //Left Right Movement
                if (Input.GetAxis("Horizontal") < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                moveVelocity = speed * Input.GetAxis("Horizontal") * Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);

                // time ability
                if (Input.GetAxis("Time") > 0)
                {
                    Time.timeScale = 0.7f ;
                    timeAmount += Time.deltaTime * 1/0.7f;
                }
                else if (timeAmount > 0)
                {
                    Time.timeScale = 1.3f;
                    timeAmount -= Time.deltaTime * 1/1.3f;
                }
                else
                {
                    Time.timeScale = 1;
                    timeAmount = 0;
                }
            }
        }

        else // if dead
        {
            GetComponent<SpriteRenderer>().color = new Color32(219, 92, 92, 255);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, 0);
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
                GetComponent<HealthManager>().TakeDamage(34);
            }
            KnockbackStart = Time.time;
            invulnerable = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, 0);


            rb.AddForce(transform.up * thrust);
            if (direction > 0)
                rb.AddForce(transform.right * -thrust);
            else if (direction < 0)
                rb.AddForce(transform.right * thrust);
        }
    }

    private void FlashDamaged()
    {
        GetComponent<SpriteRenderer>().color = new Color32(219, 92, 92, 255);
    }
}