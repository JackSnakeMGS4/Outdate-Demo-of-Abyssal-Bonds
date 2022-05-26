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

    [SerializeField]
    private GameObject impact_effect;

    private SpriteRenderer sprite;
    public SpriteRenderer Sprite { get { return sprite; } }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Destroy(gameObject, destroy_self_time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Projectile"))
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
