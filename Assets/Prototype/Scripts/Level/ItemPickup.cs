using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private Keys key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory player_inventory = collision.GetComponent<Inventory>();
        if(player_inventory != null)
        {
            player_inventory.AddKey(key);
            Destroy(gameObject, 1f);
        }
    }
}
