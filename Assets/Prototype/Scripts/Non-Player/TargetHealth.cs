using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [SerializeField, Range(1f, 1000f)]
    private float max_health = 1;
    protected float current_health;
    
    [SerializeField, Range(1f, 1000f)]
    private float max_shields = 1;
    protected float current_shields;

    [SerializeField]
    private float color_swap_delay = .3f;
    [SerializeField]
    Color hurt_color;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        current_health = max_health;
        current_shields = max_shields;
    }

    public void TakeDamage(float health_damage, float shield_damage)
    {
        if(current_shields <= 0)
        {
            StartCoroutine(DamageEffectSequence(health_damage));
        }
        else
        {
            StartCoroutine(ShieldEffectSequence(shield_damage));
        }
    }

    IEnumerator DamageEffectSequence(float damage)
    {
        sprite.color = hurt_color;
        current_health -= damage;

        yield return new WaitForSeconds(color_swap_delay);

        sprite.color = Color.white;
        if (current_health <= 0)
        {
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
    }

    IEnumerator ShieldEffectSequence(float damage)
    {
        sprite.color = Color.blue;
        current_shields -= damage;

        yield return new WaitForSeconds(color_swap_delay);

        sprite.color = Color.white;
    }

    // Following is for learning/reference
    /*public void TakeDamage()
    {
        // Tints the sprite red and fades back to the origin color after a delay of 1 second
        StartCoroutine(DamageEffectSequence(sr, Color.red, 2, 1));
    }

    IEnumerator DamageEffectSequence(SpriteRenderer sr, Color dmgColor, float duration, float delay)
    {
        // save origin color
        Color originColor = sr.color;

        // tint the sprite with damage color
        sr.color = dmgColor;

        // you can delay the animation
        yield return new WaitForSeconds(delay);

        // lerp animation with given duration in seconds
        for (float t = 0; t < 1.0f; t += Time.deltaTime / duration)
        {
            sr.color = Color.Lerp(dmgColor, originColor, t);

            yield return null;
        }

        // restore origin color
        sr.color = originColor;
    }*/
}
