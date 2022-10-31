using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using FMOD.Studio;

public class EnemyAI: MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference idle_event;
    [SerializeField]
    private FMODUnity.EventReference detection_event;

    [SerializeField, Header("Target Settings")]
    private Transform target;
    [SerializeField]
    private float aggro_range = 8f;

    [SerializeField, Header("Wander Settings")]
    private float acceleration = 2.5f;
    [SerializeField]
    private float next_waypoint_dist = 3f;
    [SerializeField]
    private float wander_radius = 5f;
    [SerializeField]
    private float wander_rate = 5f;

    private Path path;
    private int current_waypoint = 0;
    private bool reached_end_of_path = false;
    private bool is_chasing = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private EnemyShooting e_shooting;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        e_shooting = GetComponent<EnemyShooting>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdatePath", 0f, wander_rate);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, PickRandomPoint(), OnPathComplete);
            FMODUnity.RuntimeManager.PlayOneShotAttached(idle_event, gameObject);
        }
    }

    private void ChaseTarget()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

        e_shooting.ShootTarget(target);

        if (!IsTargetInRange())
        {
            is_chasing = false;
            FMODUnity.RuntimeManager.PlayOneShotAttached(idle_event, gameObject);

            CancelInvoke("ChaseTarget");
            InvokeRepeating("UpdatePath", 0f, wander_rate);
        }
    }

    private bool IsTargetInRange()
    {
        bool tgt_within_range = false;
        if(Vector2.Distance(rb.position, target.position) <= aggro_range)
        {
            tgt_within_range = true;
        }
        //Debug.Log(tgt_within_range);
        return tgt_within_range;
    }

    private Vector3 PickRandomPoint()
    {
        var point = (Vector3)Random.insideUnitCircle * wander_radius;
        point += transform.position;
        return point;
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            current_waypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }
        if(current_waypoint >= path.vectorPath.Count)
        {
            reached_end_of_path = true;
            return;
        }
        else
        {
            reached_end_of_path = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[current_waypoint] - rb.position).normalized;
        if(dir.x > 0)
        {
            sprite.flipX = false;
        }
        if(dir.x < 0)
        {
            sprite.flipX = true;
        }
        //Debug.Log(dir);
        Vector2 force = dir * acceleration;

        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[current_waypoint]);
        if(dist < next_waypoint_dist)
        {
            current_waypoint++;
        }
    }

    private void Update()
    {
        //Debug.Log(rb.velocity.magnitude);
        animator.SetBool("IsMoving", rb.velocity.magnitude > 0);

        if (IsTargetInRange() && !is_chasing)
        {
            is_chasing = true;
            FMODUnity.RuntimeManager.PlayOneShotAttached(detection_event, gameObject);

            CancelInvoke("UpdatePath");
            InvokeRepeating("ChaseTarget", 0f, 1f);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggro_range);
    }
}
