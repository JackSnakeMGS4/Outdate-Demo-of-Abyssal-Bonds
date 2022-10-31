using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public enum DamageType
{
    HP, SHI, BOTH, STANDARD_SHOT
}

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference shot_ref;
    [SerializeField]
    private FMODUnity.EventReference impact_ref;

    [SerializeField]
    private float damage = 1f;
    [SerializeField]
    private DamageType type;
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
        FMODUnity.RuntimeManager.PlayOneShotAttached(shot_ref, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(spawning_entity_tag) && !collision.CompareTag(gameObject.tag))
        {
            //Debug.Log("Damage Dealt: " + damage * percentage_vs_health + "HP / " + damage * percentage_vs_shields);
            GameObject impact_obj = Instantiate(impact_effect, transform.position, Quaternion.identity);
            impact_obj.GetComponent<SpriteRenderer>().sortingLayerName = sprite.sortingLayerName;
            FMODUnity.RuntimeManager.PlayOneShotAttached(impact_ref, collision.gameObject);

            TargetHealth target_health = collision.GetComponent<TargetHealth>();
            if (target_health != null)
            {
                target_health.TakeDamage(damage * percentage_vs_health, damage * percentage_vs_shields, type);
            }
            Destroy(impact_obj, .3f);
            Destroy(gameObject);
        }
    }
}
