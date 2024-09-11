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

    public UnityEvent<int> dayChanged;
    int _day;
    public int Day
    {
        get { return _day; }
        set 
        { 
            _day = value;
            dayChanged.Invoke(_day);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LocateModeManager(SceneManager.GetActiveScene());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (LocateModeManager(scene) && pizzaManager)
        {
            Day++;
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
                Debug.Log($"Found manager: {pizzaManager}{carManager}");
                if(carManager) carManager.gameManager = this;
                if(pizzaManager) pizzaManager.gameManager = this;
                return true;
            }
        }
        return false;
    }

}
