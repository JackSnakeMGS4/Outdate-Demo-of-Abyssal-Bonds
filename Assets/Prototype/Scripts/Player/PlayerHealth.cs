using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Range(1f, 300f)]
    private int max_health = 1;
    private int current_health;
    
    [SerializeField, Range(1f, 300f)]
    private int max_shields = 1;
    private int current_shields;

    [SerializeField]
    private TextMeshProUGUI health_text;
    [SerializeField]
    private TextMeshProUGUI shields_text;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        current_shields = max_shields;
    }

    // Update is called once per frame
    void Update()
    {
        health_text.text = "Health: " + current_health.ToString();
        shields_text.text = "Shields: " + current_shields.ToString();
    }
}
