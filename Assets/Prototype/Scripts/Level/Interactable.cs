using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private AreaLock area;

    // Start is called before the first frame update
    void Start()
    {
        area = GetComponent<AreaLock>();   
    }

    public void Interact(Keys key)
    {
        if(key.unlocks == area.Lock_Name)
        {
            gameObject.SetActive(false);
        }
    }
}
