using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // reference to the game object
    
    public static bool GamePaused = false; // checks if the game is paused

    void Update()
    {
        // if the user presses the escape key then pause or resume the game
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GamePaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // resume gets rid of the pause menu UI and resume game time 
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        PauseAudio();
    }

    void Pause()
    {
        // pause opens pause menu UI and stops game time 
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        PauseAudio();
    }

    public void LoadMenu() 
    { 
        // resume game time and allows user to go back to the main menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void PauseAudio()
    {
        // makes sure all audio is paused when the game is paused 
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio)
        {
            if (a.isActiveAndEnabled == true)
            {
                if (a.isPlaying) a.Pause();
                else a.UnPause();
            }
        }

    }
}
