using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergy", menuName = "ScriptableObjects/New Synergy", order = 2)]
public class Synergy : ScriptableObject
{
    public Card[] card = new Card[3];
}
