using System.Collections;
using System.Collections.Generic;
using PizzaOrder;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour
{
    #region singleton

    public static GameManager singleton;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public PizzaModeManager pizzaManager;
    public CarModeManager carManager;
    public UIManager UIManager;
    public FadeInOut fade;
    
    /// <summary>
    /// used to transfer the pizzas between the pizza and car modes
    /// </summary>
    public uint pizzaCount;

    public string mainSceneName; 
    public string endOfDayScene;
    public string endGameScene;
    [SerializeField] string pizzaSceneName;
    public string carSceneName;
    [SerializeField] AudioSource ambiancePlayer;
    public UnityEvent<int> dayChanged;
    int _day;
    private int _quota;
    public UnityEvent<int> quotaChanged;

    public bool enableTutorial;
    
    [HideInInspector] public UnityEvent pauseGame;
    [HideInInspector] public UnityEvent resumeGame;

    [HideInInspector] public UnityEvent tutorialMenuInit;
    
    [HideInInspector] public UnityEvent carModeInit;
    [HideInInspector] public UnityEvent pizzaModeInit;

    [SerializeField] uint[] scoreRequiredToPass;
    
    public List<Order> OrdersToDeliver = new();

    public int daysNeededToWin = 5;
    public enum GameState
    {
        ongoing,
        victory,
        loss
    }
    public GameState gameState;
    
    public int Day
    {
        get { return _day; }
        set 
        { 
            _day = value;
            //Debug.Log("[GAME MANAGER]: Current Day: " + _day);
            dayChanged.Invoke(_day);
        }
    }

    public int Quota
    {
        get { return _quota; }
        set
        {
            _quota = value;
            quotaChanged.Invoke(_quota);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    //private void CheckWinCondition(int day)
    //{
    //    if (day < daysNeededToWin) return;
    //    gameState = GameState.victory;
    //}
    
    public void UnscoreNextPizza()
    {
        Debug.Log($"UNSCORING PIZZA: {OrdersToDeliver.Count}");
        for (int i = 0; i < OrdersToDeliver.Count; i++)
        {
            if (OrdersToDeliver[i].validForScoring)
            {
                OrdersToDeliver[i].validForScoring = false;
                break;
            }

        }

        string logString = "[orderReport]: ";
        for (int i = 0; i < OrdersToDeliver.Count; i++)
        {
            logString += $"Order: {OrdersToDeliver[i].name} | Score: {OrdersToDeliver[i].score} | Valid: {OrdersToDeliver[i].validForScoring}\n";
        }
        Debug.Log(logString);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(fadeOutTransition());
        
        if (LocateModeManager(scene))
        {
            InitializePizzaMode();
            InitializeCarMode();
        }
    }

    bool LocateModeManager(Scene scene)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            carManager = root.GetComponentInChildren<CarModeManager>();
            pizzaManager = root.GetComponentInChildren<PizzaModeManager>();
            if (pizzaManager || carManager)
            {
                return true;
            }
        }
        return false;
    }

    void InitializeCarMode()
    {
        if (carManager)
        {
            carModeInit.Invoke();
            carManager.gameManager = this;
            carManager.PizzasToDeliver = pizzaCount;
            if(!ambiancePlayer.isPlaying) ambiancePlayer.Play();
            ambiancePlayer.spatialBlend = 0.9f;
            ambiancePlayer.minDistance = 50;
            transform.position = carManager.ambianceSoundLocation.position;

            OrdersToDeliver.Sort((o, o1) => o.score.CompareTo(o1.score));

        }
    }

    void InitializePizzaMode()
    {
        if (pizzaManager)
        {
            ++Day;
            pizzaModeInit.Invoke();
            pizzaManager.gameManager = this;
            ambiancePlayer.spatialBlend = 0.9f;
            ambiancePlayer.minDistance = 1;
            transform.position = pizzaManager.ambianceSoundLocation.position;
            OrdersToDeliver.Clear();
        }
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator fadeOutTransition()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1f);
    }

    public void PostCarGame()
    {

        Debug.Log("[POST CAR GAME] Scoring Pizzas");
        Debug.Log($"[POST CAR GAME] Score Needed: {scoreRequiredToPass[Mathf.Clamp(Day - 1, 0, daysNeededToWin)]}");
        float totalScore = 0;

        for (int i = 0; i < OrdersToDeliver.Count; i++)
        {
            if (OrdersToDeliver[i].validForScoring)
            {
                totalScore += OrdersToDeliver[i].score;
                Debug.Log($"[POST CAR GAME]pizza {i} Score: {OrdersToDeliver[i].score}");
            }
        }
        Debug.Log($"[POST CAR GAME] RESULT: {totalScore} / {scoreRequiredToPass[Mathf.Clamp(Day - 1, 0, daysNeededToWin)]}");

        if (totalScore < scoreRequiredToPass[Mathf.Clamp(Day-1, 0 , daysNeededToWin)])
        {
            Debug.Log("[POST CAR GAME] lost");
            gameState = GameState.loss;
        }
        else if(Day >= daysNeededToWin)
        {
            Debug.Log("[POST CAR GAME] won");
            gameState = GameState.victory;
        }
        else
        {
            Debug.Log("[POST CAR GAME] gonna keep goin");
            gameState = GameState.ongoing;
        }



        switch (gameState)
        {
            case GameState.ongoing:
                LoadDayOver();
                break;
            case GameState.victory:
                LoadEndGame();
                break;
            case GameState.loss:
                LoadEndGame();
                break;
        }
    }
    
    #region SceneLoading
    public void LoadPizzaScene()
    {
        StartCoroutine(ChangeScene(pizzaSceneName));
    }

    public void LoadDayOver()
    {
        StartCoroutine(ChangeScene(endOfDayScene));
    }

    public void LoadEndGame()
    {
        StartCoroutine(ChangeScene(endGameScene));
    }
    public void LoadMenuScene()
    {
        StartCoroutine(ChangeScene(mainSceneName));
    }
    public void LoadCarScene()
    {
        StartCoroutine(ChangeScene(carSceneName));
    }
    #endregion SceneLoading
}
