using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Keys> keys = new List<Keys>();
    public List<Keys> My_Keys
    {
        get { return keys; }
    }

    public void AddKey(Keys key)
    {
        keys.Add(key);
    }
}
