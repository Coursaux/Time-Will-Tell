using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyGear : MonoBehaviour
{
    //where to path to
    public Transform target;
    // how many times/second to update path
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;
    public float speed = 300;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float detectRange = 8;
    public float attackRange = 4;

    float smooth = 5.0f;

    private float attackLength = 1;
    private float attackStart = -10f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("no player found");
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: Insert a player search here
            yield return false;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("error? " + p.error + p.errorLog);
        path = p;
        if (!p.error)
        {
            currentWaypoint = 0;
            return;
        }
    }

    public void FixedUpdate()
    {
        Attack();
        Move();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            // ends attack
            attackStart -= 2;

            Transform sparksRight = transform.GetChild(3);
            Transform sparksLeft = transform.GetChild(4);
            if (sparksRight.GetComponent<ParticleSystem>().isPlaying)
            {
                sparksRight.GetComponent<ParticleSystem>().Stop();
            }
            else if (sparksLeft.GetComponent<ParticleSystem>().isPlaying)
            {
                sparksLeft.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    private void Move()
    {
        // pathing
        if (target == null)
        {
            //TODO: Insert a player search here
            return;
        }
        if (path == null)
        {
            return;
        }
        // move and spin
        float pathLength = path.GetTotalLength();
        if (pathLength < detectRange)
        {
            //spin left
            if ((transform.position - target.transform.position).x > 0)
            {
                Transform gear = transform.GetChild(1).transform;
                Quaternion target = gear.rotation * Quaternion.Euler(0, 0, 30);
                gear.rotation = Quaternion.Slerp(gear.rotation, target, Time.fixedDeltaTime * smooth);
            }
            // spin righ
            else
            {
                Transform gear = transform.GetChild(1).transform;
                Quaternion target = gear.rotation * Quaternion.Euler(0, 0, -30);
                gear.rotation = Quaternion.Slerp(gear.rotation, target, Time.fixedDeltaTime * smooth);
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                {
                    return;
                }
                Debug.Log("end of path");
                pathIsEnded = true;
                return;
            }
            pathIsEnded = false;

            // move
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;
            rb.AddForce(dir, fMode);

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (dist < nextWaypointDistance)
            {
                currentWaypoint++;
                return;
            }
        }
    }

    private void Attack()
    {
        if ((transform.position - target.transform.position).y < -0.04 && (transform.position - target.transform.position).y > -0.07 && 
            ((transform.position - target.transform.position).x < attackRange && (transform.position - target.transform.position).x > -attackRange)) // if in range and level with player
            {
            if ((transform.position - target.transform.position).x >= 0) // right/left check
            {
                if (Time.time < attackStart + attackLength)
                {
                    Transform gear = transform.GetChild(1).transform;
                    Quaternion target = gear.rotation * Quaternion.Euler(0, 0, 100);
                    gear.rotation = Quaternion.Slerp(gear.rotation, target, Time.fixedDeltaTime * smooth);

                    Vector3 dir = new Vector3(-1, 0, 0);
                    dir *= speed * 4 * Time.fixedDeltaTime;
                    rb.AddForce(dir, fMode);
                }
                if (attackStart + 4 < Time.time)
                {
                    Transform sparks = transform.GetChild(3);
                    attackStart = Time.time;
                    sparks.GetComponent<ParticleSystem>().Play();

                    Vector3 dir = new Vector3(-1, 0, 0);
                    dir *= speed * 4 * Time.fixedDeltaTime;
                    rb.AddForce(dir, fMode);
                }
            }
            else
            {
                if (Time.time < attackStart + attackLength)
                {
                    Transform gear = transform.GetChild(1).transform;
                    Quaternion target = gear.rotation * Quaternion.Euler(0, 0, -100);
                    gear.rotation = Quaternion.Slerp(gear.rotation, target, Time.fixedDeltaTime * smooth);

                    Vector3 dir = new Vector3(1, 0, 0);
                    dir *= speed * 3 * Time.fixedDeltaTime;
                    rb.AddForce(dir, fMode);
                }
                if (attackStart + attackLength + 2 < Time.time)
                {
                    Transform sparks = transform.GetChild(4);
                    attackStart = Time.time;
                    sparks.GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }
}
