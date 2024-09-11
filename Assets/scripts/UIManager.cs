using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject carGameUI;
    public GameObject pizzaGameUI;
    public GameObject carGameControls;
    public GameObject pizzaGameControls;
    
    private GameManager gameManager;

    private bool gameIsPaused;
    private bool openPauseMenu;
    
    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        
        gameManager.pauseGame.AddListener(PauseGame);
        gameManager.resumeGame.AddListener(UnPauseGame);
        
        gameManager.carModeInit.AddListener(InitializeCarGameUI);
        gameManager.pizzaModeInit.AddListener(InitializePizzaGameUI);
    }

    private void InitializeCarGameUI()
    {
        SwitchUI(pizzaGameUI, carGameUI);
        SwitchUI(pizzaGameControls, carGameControls);
    }

    private void InitializePizzaGameUI()
    {
        SwitchUI(carGameUI, pizzaGameUI);
        SwitchUI(carGameControls, pizzaGameControls);
    }

    private static void SwitchUI(GameObject prevObj, GameObject uiObject)
    {
        prevObj.SetActive(false);
        uiObject.SetActive(true);
    }

    #region Pause Menu

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

    #endregion
}
