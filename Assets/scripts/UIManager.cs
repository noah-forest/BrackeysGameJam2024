using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    
    private GameManager gameManager;

    private bool gameIsPaused;
    private bool openPauseMenu;
    
    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        
        gameManager.pauseGame.AddListener(PauseGame);
        gameManager.resumeGame.AddListener(UnPauseGame);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == gameManager.mainSceneName) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void BackToMainMenu()
    {
        gameManager.resumeGame.Invoke();
        gameManager.LoadMenuScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void PauseGame()
    {
        gameIsPaused = true;
        pauseMenu.SetActive(gameIsPaused);
        Time.timeScale = 0;
    }

    private void UnPauseGame()
    {
        gameIsPaused = false;
        pauseMenu.SetActive(gameIsPaused);
        Time.timeScale = 1;
    }

    public void TogglePauseMenu()
    {
        if (gameIsPaused)
        {
            openPauseMenu = false;
            gameManager.resumeGame.Invoke();
        }
        else
        {
            openPauseMenu = true;
            gameManager.pauseGame.Invoke();
        }
    }
}
