using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PauseManager : MonoBehaviour
{
    public static bool is_paused = false;

    public void DeterminePause()
    {
        if (is_paused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        is_paused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        is_paused = false;
    }

    public void MasterBusVol()
    {

    }

    public void MusicBusVol()
    {

    }
    
    public void SoundBusVol()
    {

    }
}
