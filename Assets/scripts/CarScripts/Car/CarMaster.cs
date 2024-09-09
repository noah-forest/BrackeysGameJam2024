using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMaster : MonoBehaviour
{
    public Rigidbody body;
    public CarHealth health;
    public CarController controller;
    public CarInteractor interactor;
    public CarParticleManager particleManager;
    public Camera sceneCamera;

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
