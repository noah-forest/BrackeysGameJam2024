using System.Collections;
using System.Collections.Generic;
using PizzaOrder;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


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
    [SerializeField] string pizzaSceneName;
    [SerializeField] string carSceneName;
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
    
    public List<Order> completedOrders = new();
    
    public int Day
    {
        get { return _day; }
        set 
        { 
            _day = value;
            Debug.Log("[GAME MANAGER]: Current Day: " + _day);
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
    
    #region SceneLoading
    public void LoadPizzaScene()
    {
        StartCoroutine(ChangeScene(pizzaSceneName));
    }

    public void LoadDayOver()
    {
        StartCoroutine(ChangeScene(endOfDayScene));
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
