using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference walk_ref;
    [SerializeField]
    private FMODUnity.EventReference dash_ref;

    [SerializeField, Range(1f,50f), Header("Movement Properties")]
    private float max_speed = 1f;
    [SerializeField, Range(1f, 50f)]
    private float acceleration = 1f;
    [SerializeField, Range(1f, 50f)]
    private float acceleration_aiming = 1f;

    [SerializeField, Range(1f, 50f), Header("Dashing Properties")]
    private float dash_speed = 1f;
    [SerializeField]
    private float dash_limit;
    private float dash_time;
    private bool is_dashing = false;
    [SerializeField]
    private float dash_cooldown = 3f;
    private float cooldown_time;
    private bool can_dash = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        dash_time = dash_limit;
        cooldown_time = dash_cooldown;
    }

    private void Update()
    {
        DashTimer();
        DashCooldown(); 
    }

    private void DashCooldown()
    {
        if (!can_dash)
        {
            dash_cooldown -= Time.deltaTime;
            if(dash_cooldown < 0)
            {
                can_dash = true;
                dash_cooldown = cooldown_time;
            }
        }
    }

    private void DashTimer()
    {
        if (is_dashing)
        {
            dash_time -= Time.deltaTime;
            if (dash_time < 0)
            {
                is_dashing = false;
                dash_time = dash_limit;
            }
        }
    }

    public void Move(Vector2 vector2, bool is_aiming)
    {
        if (!is_dashing)
        {
            float horizontal_axis = vector2.x * (is_aiming ? acceleration_aiming : acceleration);
            float vertical_axis = vector2.y * (is_aiming ? acceleration_aiming : acceleration);

            if (horizontal_axis > 0)
            {
                //animator.SetInteger("Direction", 2);
                sprite.flipX = false;
            }
            if (horizontal_axis < 0)
            {
                //animator.SetInteger("Direction", 3);
                sprite.flipX = true;
            }
            //if (vertical_axis > 0) animator.SetInteger("Direction", 1);
            //if (vertical_axis < 0) animator.SetInteger("Direction", 0);

            //limit movement speed to max speed

            Vector2 new_vel = new Vector2(horizontal_axis, vertical_axis);
            rb.velocity = new_vel;

            new_vel.Normalize();
            animator.SetBool("IsMoving", new_vel.magnitude > 0);
        }
    }

    public void Dash(Vector2 vector)
    {
        //Debug.Log("Dashing");
        if(can_dash)
        {
            is_dashing = true;
            can_dash = false;
            rb.AddForce(vector * dash_speed, ForceMode2D.Impulse);
            FMODUnity.RuntimeManager.PlayOneShot(dash_ref);
        }
    }
}
