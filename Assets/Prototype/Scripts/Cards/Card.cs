using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 1)]
public class Card: ScriptableObject
{
    [SerializeField]
    private new string name;
    [SerializeField, TextArea]
    private string description;
    [SerializeField, Tooltip("0 is Attack, 1 is Support, and 2 is Special")]
    private int card_type;

    [SerializeField]
    private Sprite card_art;

    [SerializeField]
    private float percentage_vs_health;
    [SerializeField]
    private float percentage_vs_shields;
}
