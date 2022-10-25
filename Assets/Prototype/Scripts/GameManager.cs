using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI objective_text;

    [SerializeField] 
    private GameObject boss;
    [SerializeField] 
    private GameObject player;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!boss.activeSelf)
        {
            objective_text.text = "Great Sentience Destroyed";
        }
        else if (!player.activeSelf)
        {
            objective_text.text = "Game Over";
        }
    }
}
