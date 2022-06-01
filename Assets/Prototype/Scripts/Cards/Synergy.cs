using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergy", menuName = "ScriptableObjects/New Synergy", order = 2)]
public class Synergy : ScriptableObject
{
    [TextArea]
    public string synergy_behaviour;

    public GameObject salvo;

    public Card[] required_cards = new Card[3];
}
