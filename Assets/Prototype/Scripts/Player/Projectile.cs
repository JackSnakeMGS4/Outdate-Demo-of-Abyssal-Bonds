using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float destroy_self_time = 10f;

    private Rigidbody2D rb;
    private Vector3 force;
    private float firing_velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveProjectile();
        Destroy(gameObject, destroy_self_time);
    }

    private void MoveProjectile()
    {
        rb.AddForce(force * firing_velocity, ForceMode2D.Impulse);
    }

    public void ProjectileSettings(Vector3 dir, float firing_vel)
    {
        force = dir;
        firing_velocity = firing_vel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
