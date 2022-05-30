using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//84 unique synergies
public enum Card_Types
{
    hollow_point, laser_shot, energy_blast,
    drain, stasis, decoy,
    teleport, stun_blast, trackers
}

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/New Card", order = 1)]
public class Card: ScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public Card_Types type;

    public Sprite card_art;
    public GameObject projectile_type;

    public float percentage_vs_health;
    public float percentage_vs_shields;

    public Synergy[] synergies;
}
