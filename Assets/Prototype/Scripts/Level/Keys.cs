using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "ScriptableObjects/New Key", order = 3)]
public class Keys : ScriptableObject
{
    public Locks unlocks;
}
