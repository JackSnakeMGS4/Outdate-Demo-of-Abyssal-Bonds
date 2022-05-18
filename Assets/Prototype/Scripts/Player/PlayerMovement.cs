using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1f,50f)]
    private float max_speed = 1f;
    [SerializeField, Range(1f, 50f)]
    private float acceleration = 1f;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 vector2)
    {
        float horizontal_axis = vector2.x * acceleration;
        float vertical_axis = vector2.y * acceleration;

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
