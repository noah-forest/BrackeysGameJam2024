using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarMaster : MonoBehaviour
{
    public CarModeManager modeManager;
    public Camera sceneCamera;
    public Rigidbody body;
    public CarHealth health;
    public CarController controller;
    public CarInteractor interactor;
    public CarParticleManager particleManager;
    public GoalCompass compass;

    #region singleton

    public static CarMaster singleton;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }

        singleton = this;
    }
    #endregion

}
