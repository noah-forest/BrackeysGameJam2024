using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    PizzaModeManager pizzaManager;
    CarModeManager carManager;

    /// <summary>
    /// used to transfer the pizzas between the pizza and car modes
    /// </summary>
    public uint pizzaCount;

    public string mainSceneName;
    [SerializeField] UIManager UIManager;
    [SerializeField] string pizzaSceneName;
    [SerializeField] string carSceneName;
    [SerializeField] AudioSource ambiancePlayer;
    public UnityEvent<int> dayChanged;
    int _day;

    [HideInInspector] public UnityEvent pauseGame;
    [HideInInspector] public UnityEvent resumeGame;
    
    public int Day
    {
        get { return _day; }
        set 
        { 
            _day = value;
            Debug.Log(_day);
            dayChanged.Invoke(_day);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
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
            pizzaManager.gameManager = this;
            ambiancePlayer.spatialBlend = 0.9f;
            ambiancePlayer.minDistance = 1;
            transform.position = pizzaManager.ambianceSoundLocation.position;
        }
    }

    #region SceneLoading
    public void LoadPizzaScene()
    {
        SceneManager.LoadScene(pizzaSceneName);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(mainSceneName);
    }
    public void LoadCarScene()
    {
        SceneManager.LoadScene(carSceneName);
    }
    #endregion SceneLoading
}
