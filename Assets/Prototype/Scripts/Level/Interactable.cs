using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private GameObject context_icon;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            context_icon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            context_icon.SetActive(false);
        }
    }
}
