using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerHealth : TargetHealth
{
    [SerializeField]
    private TextMeshProUGUI health_text;
    [SerializeField]
    private TextMeshProUGUI shields_text;
    [SerializeField]
    private TextMeshProUGUI shield_boost_text;

    [SerializeField]
    private float shield_boost_value = 20f;
    [SerializeField]
    private float recharge_cooldown = 6f;
    private float cooldown_timer;

    private Rigidbody2D rb;
    private bool can_recharge = true;
    private bool is_recharging = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cooldown_timer = recharge_cooldown;
    }

    void Update()
    {
        health_text.text = "Health: " + current_health.ToString();
        shields_text.text = "Shields: " + current_shields.ToString();
        shield_boost_text.text = "Shield Boost: " + (can_recharge ? "Ready" : "Cooldown");

        ShieldCooldown();
    }

    private void ShieldCooldown()
    {
        if (!can_recharge)
        {
            cooldown_timer -= Time.deltaTime;
            if(cooldown_timer < 0)
            {
                can_recharge = true;
                cooldown_timer = recharge_cooldown;
            }
        }
    }

    public void RechargeShield()
    {
        if(current_shields < max_shi && can_recharge)
        {
            can_recharge = false;
            current_shields += shield_boost_value;
        }
    }
}
