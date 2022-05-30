using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : TargetHealth
{
    [SerializeField]
    private TextMeshProUGUI health_text;
    [SerializeField]
    private TextMeshProUGUI shields_text;

    void Update()
    {
        health_text.text = "Health: " + current_health.ToString();
        shields_text.text = "Shields: " + current_shields.ToString();
    }
}
