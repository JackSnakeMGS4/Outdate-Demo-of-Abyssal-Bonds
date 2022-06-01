using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropKey : MonoBehaviour
{
    [SerializeField]
    private GameObject item;

    private void OnDisable()
    {
        Instantiate(item, transform.position, transform.rotation);
    }
}
