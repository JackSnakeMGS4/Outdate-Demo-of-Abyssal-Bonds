using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private float destroy_self_time = 10f;
    private float percentage_vs_health = 1f;
    private float percentage_vs_shields = 1f;
    private float default_percentage = 1f;

    [SerializeField]
    private GameObject impact_effect;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private string spawning_entity_tag;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Destroy(gameObject, destroy_self_time);
    }

    public void ProjectileSettings(string sorting_layer, float hp_dmg_percent, float shield_dmg_percent, Vector2 dir, float fire_vel, string origin_tag)
    {
        sprite.sortingLayerName = sorting_layer;
        percentage_vs_health = hp_dmg_percent/100;
        percentage_vs_shields = shield_dmg_percent/100;
        spawning_entity_tag = origin_tag;

        rb.AddForce(dir * fire_vel, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(spawning_entity_tag) && !collision.CompareTag(gameObject.tag))
        {
            GameObject impact_obj = Instantiate(impact_effect, transform.position, Quaternion.identity);
            impact_obj.GetComponent<SpriteRenderer>().sortingLayerName = sprite.sortingLayerName;

            TargetHealth target_health = collision.GetComponent<TargetHealth>();
            if (target_health != null)
            {
                target_health.TakeDamage(damage * percentage_vs_health, damage * percentage_vs_shields);
            }
            Destroy(impact_obj, .2f);
            Destroy(gameObject);
        }
    }
}
