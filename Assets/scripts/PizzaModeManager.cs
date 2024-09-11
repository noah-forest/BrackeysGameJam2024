using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaModeManager : MonoBehaviour
{
    #region singleton

    public static PizzaModeManager singleton;

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
    public PlayerController player;
    public GameManager gameManager;
}
