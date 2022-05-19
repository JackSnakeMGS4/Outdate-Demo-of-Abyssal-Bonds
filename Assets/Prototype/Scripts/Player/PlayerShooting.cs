using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField, Header("Aiming Fields")]
    private Sprite reticle;
    [SerializeField, Range(1f, 10f)]
    private float range = 5f;
    [SerializeField]
    private Transform fire_point;

    private Vector3 aim_pos;

    private bool is_aiming = false;
    public bool Is_Aiming
    {
        get { return is_aiming; }
    }

    [SerializeField,Header("Projectile Fields")]
    private GameObject standard_shot;
    [SerializeField, Range(1f, 10f)]
    private float firing_velocity;

    public void AimCursor(Vector2 vector2)
    {
        //NOTE: needs to be handled differently for mouse
        //Debug.Log(vector2);
        if (vector2.magnitude > 0)
        {
            //Debug.DrawLine(transform.position, transform.position + new Vector3(vector2.x, vector2.y, 0) * draw_range, Color.yellow);

            is_aiming = true;
            aim_pos = new Vector3(transform.position.x + vector2.x, transform.position.y + vector2.y, 0);
            //draw aiming sprite in input direction in relation to player position and range field
            //draw line to aim point
            Debug.DrawLine(transform.position, aim_pos,Color.yellow);
            //set firing point direction to aim direction
            //NOTE: might not need firing point for this project
            fire_point.position = aim_pos;
        }
        else
        {
            aim_pos = transform.position;
            fire_point.position = aim_pos;
            is_aiming = false;
        }
    }

    public void Shoot()
    {
        if (is_aiming)
        {
            //shoot projectile in shot direction
            Vector2 dir = aim_pos - transform.position;
            dir.Normalize();

            GameObject obj = Instantiate(standard_shot, fire_point.position, fire_point.rotation);
            Projectile script = obj.GetComponent<Projectile>();
            script.ProjectileSettings(dir, firing_velocity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(aim_pos, .2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fire_point.position, .2f);
    }
}
