using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1f,50f), Header("Movement Properties")]
    private float max_speed = 1f;
    [SerializeField, Range(1f, 50f)]
    private float acceleration = 1f;
    [SerializeField, Range(1f, 50f)]
    private float acceleration_aiming = 1f;

    [SerializeField, Range(1f, 50f), Header("Dashing Properties")]
    private float dash_speed = 1f;
    [SerializeField]
    private float startDashTime;
    private float dash_time;
    private bool is_dashing = false;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dash_time = startDashTime;
    }

    private void Update()
    {
        if (is_dashing)
        {
            dash_time -= Time.deltaTime;
            if(dash_time < 0)
            {
                is_dashing = false;
                dash_time = startDashTime;
            }
        }
    }

    public void Move(Vector2 vector2, bool is_aiming)
    {
        if (!is_dashing)
        {
            float horizontal_axis = vector2.x * (is_aiming ? acceleration_aiming : acceleration);
            float vertical_axis = vector2.y * (is_aiming ? acceleration_aiming : acceleration);

            if (horizontal_axis > 0) animator.SetInteger("Direction", 2);
            if (horizontal_axis < 0) animator.SetInteger("Direction", 3);
            if (vertical_axis > 0) animator.SetInteger("Direction", 1);
            if (vertical_axis < 0) animator.SetInteger("Direction", 0);

            //limit movement speed to max speed

            Vector2 new_vel = new Vector2(horizontal_axis, vertical_axis);
            rb.velocity = new_vel;

            new_vel.Normalize();
            animator.SetBool("IsMoving", new_vel.magnitude > 0);
        }
    }

    public void Dash(Vector2 vector)
    {
        Debug.Log("Dashing");
        is_dashing = true;
        rb.AddForce(vector * dash_speed, ForceMode2D.Impulse);
    }
}
