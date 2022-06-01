using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Locks
{
    boss_gate, treasure_chest
}

public class AreaLock : MonoBehaviour
{
    [SerializeField]
    private Locks lock_name;
    public Locks Lock_Name
    {
        get { return lock_name; }
    }
}
