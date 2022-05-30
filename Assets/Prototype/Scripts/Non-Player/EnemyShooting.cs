using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField]
    private Transform fire_point;

    [SerializeField, Header("Projectile Fields")]
    private GameObject standard_shot;
    [SerializeField, Range(1f, 30f)]
    private float firing_velocity = 20f;

    [SerializeField, Header("Gun Settings")]
    private int max_ammo = 9;
    private int current_ammo;
    [SerializeField, Range(0f, 5f)]
    private float reload_time = 2.5f;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        current_ammo = max_ammo;
    }

    public void ShootTarget(Transform tgt)
    {
        if(current_ammo > 0)
        {
            Vector2 dir = tgt.position - fire_point.position;
            dir.Normalize();

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            fire_point.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject bullet = Instantiate(standard_shot, fire_point.position, fire_point.rotation);
            bullet.layer = gameObject.layer;
            Projectile projectile = bullet.GetComponent<Projectile>();
            projectile.ProjectileSettings(sprite.sortingLayerName, 100f, 100f, dir, firing_velocity, gameObject.tag);

            current_ammo--;

            if (current_ammo <= 0)
            {
                StartCoroutine(EnemyReload());
            }
        }
    }

    IEnumerator EnemyReload()
    {
        //effects/animation
        yield return new WaitForSeconds(reload_time);
        current_ammo = max_ammo;
    }
}
