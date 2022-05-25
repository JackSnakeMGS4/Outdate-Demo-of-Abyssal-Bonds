using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 1)]
public class Card: ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    [Tooltip("0 is Attack, 1 is Support, and 2 is Special")]
    public int card_type;

    public Sprite card_art;

    public float percentage_vs_health;
    public float percentage_vs_shields;
}
