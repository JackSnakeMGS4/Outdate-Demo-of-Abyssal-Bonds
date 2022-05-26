using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    [SerializeField, Range(1f, 30f)]
    private float firing_velocity = 20f;

    [SerializeField, Header("Gun Settings")]
    private int max_ammo = 9;
    private int current_ammo;
    [SerializeField, Range(0f, 5f)]
    private float reload_time = 2.5f;

    [SerializeField]
    private TextMeshProUGUI ammo_count_text;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        current_ammo = max_ammo;
    }

    private void Update()
    {
        ammo_count_text.text = "Ammo: " + current_ammo.ToString();
    }

    public void AimCursor(Vector2 vector2)
    {
        //NOTE: needs to be handled differently for mouse
        //Debug.Log(vector2);
        if (vector2.magnitude > 0.01f)
        {
            // Fix direction angle bug; probably have to use mathf.atan for it?

            is_aiming = true;
            aim_pos = new Vector3(fire_point.position.x + vector2.x * range, fire_point.position.y + vector2.y * range, 0);
            //draw aiming sprite in input direction in relation to player position and range field
            //draw line to aim point
            Debug.DrawLine(fire_point.position, aim_pos, Color.yellow);
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

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            fire_point.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject projectile = Instantiate(standard_shot, fire_point.position, fire_point.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.AddForce(dir * firing_velocity, ForceMode2D.Impulse);

            projectile.GetComponent<Projectile>().Sprite.sortingLayerName = sprite.sortingLayerName;

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
