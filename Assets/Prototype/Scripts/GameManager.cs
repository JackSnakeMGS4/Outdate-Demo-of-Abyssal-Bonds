using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FMODUnity.EventReference ambience_event;
    private EventInstance ambience_state;

    [SerializeField]
    private TextMeshProUGUI objective_text;

    [SerializeField] 
    private GameObject boss;
    [SerializeField] 
    private GameObject player;

    [SerializeField]
    private int enemies_left_before_boss;
    public int en_left { get { return enemies_left_before_boss; } set { enemies_left_before_boss = value; } }

    private bool boss_defeated = false;
    public bool is_boss_dead { get { return boss_defeated; } set { boss_defeated = value; } }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        ambience_state = FMODUnity.RuntimeManager.CreateInstance(ambience_event);
        ambience_state.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ambience_state, player.transform);
    }

    private void Update()
    {
        if (en_left <= 0)
        {
            SpawnBoss();
        }

        if (boss_defeated)
        {
            objective_text.text = "Great Sentience Destroyed (Thanks for playing my prototype!)";
        }
        else if (!player.activeSelf)
        {
            objective_text.text = "Game Over";
        }
    }

    private void SpawnBoss()
    {
        if (!boss.activeSelf && !boss_defeated)
        {
            objective_text.text = "A Great Sentience has appeared";
            boss.SetActive(true);
        }
    }
}
