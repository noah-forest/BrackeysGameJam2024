using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
	public MainMenuManager pauseMenuManager;
    public GameObject tutorialMenu;

    private GameManager gameManager;

    public bool gameIsPaused { get; private set; }
    private bool openPauseMenu;

    private bool tutorialOpen;
    
    private void Start()
    {
        gameManager = GameManager.singleton;

		if (gameManager.enableTutorial)
        {
            gameManager.pizzaModeInit.AddListener(SetUpTutorial);
        }
        
        gameManager.pauseGame.AddListener(PauseGame);
        gameManager.resumeGame.AddListener(UnPauseGame);
 
    }

    private void SetUpTutorial()
    {
        if (gameManager.Day != 1) return;
        //Debug.Log("its first day, pause game");
        tutorialMenu.SetActive(true);
        Time.timeScale = 0;
        tutorialOpen = true;
        gameIsPaused = true;
        gameManager.tutorialMenuInit.Invoke();
    }

    public void CloseTutorial()
    {
        tutorialMenu.SetActive(false);
        Time.timeScale = 1;
        tutorialOpen = false;
        gameIsPaused = false;
        gameManager.resumeGame.Invoke();
    }
    
    #region Pause Menu

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == gameManager.mainSceneName) return;
        if (SceneManager.GetActiveScene().name == gameManager.endOfDayScene) return;
        if (SceneManager.GetActiveScene().name == gameManager.endGameScene) return;
        if (tutorialOpen) return;

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
		// game is paused
        if (gameIsPaused)
        {
			pauseMenuManager.CloseMenus();
			openPauseMenu = false;
            if (SceneManager.GetActiveScene().name == gameManager.carSceneName)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            gameManager.resumeGame.Invoke();
        }
        else //pause game
        {
			openPauseMenu = true;
            Cursor.lockState = CursorLockMode.Confined;
            gameManager.pauseGame.Invoke();
        }
    }

    #endregion
}
