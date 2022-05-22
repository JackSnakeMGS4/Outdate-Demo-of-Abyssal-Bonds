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

    [SerializeField, Header("Projectile Fields")]
    private GameObject standard_shot;
    [SerializeField, Range(1f, 10f)]
    private float firing_velocity;

    [SerializeField, Header("Gun Settings")]
    private int max_ammo = 9;
    private int current_ammo;
    [SerializeField, Range(0f, 5f)]
    private float reload_time = 2.5f;

    private void Start()
    {
        current_ammo = max_ammo;
    }

    public void AimCursor(Vector2 vector2)
    {
        //NOTE: needs to be handled differently for mouse
        //Debug.Log(vector2);
        if (vector2.magnitude > 0.01f)
        {
            // Fix direction angle bug; probably have to use mathf.atan for it?

            is_aiming = true;
            aim_pos = new Vector3(fire_point.position.x + vector2.x, fire_point.position.y + vector2.y, 0);
            //draw aiming sprite in input direction in relation to player position and range field
            //draw line to aim point
            Debug.DrawLine(fire_point.position, aim_pos,Color.yellow);
        }
        else
        {
            aim_pos = fire_point.transform.position;
            //fire_point.position = aim_pos;
            is_aiming = false;
        }
    }

    public void Shoot()
    {
        if (is_aiming && current_ammo > 0)
        {
            //shoot projectile in shot direction
            Vector2 dir = aim_pos - fire_point.position;
            dir.Normalize();

            GameObject obj = Instantiate(standard_shot, fire_point.position, fire_point.rotation);
            Projectile script = obj.GetComponent<Projectile>();
            script.ProjectileSettings(dir, firing_velocity);
            current_ammo--;

            if(current_ammo <= 0)
            {
                Debug.Log("Auto reloading");
                StartCoroutine(AutoReload());
            } 
        }
    }

    IEnumerator AutoReload()
    {
        //effects/animation
        yield return new WaitForSeconds(reload_time);
        Debug.Log("Reload done");
        current_ammo = max_ammo;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(aim_pos, .2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fire_point.position, .2f);
    }
}
