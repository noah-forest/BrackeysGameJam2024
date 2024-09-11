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
    /// <summary>
    /// When the player leaves the pizzaria, This value is passed to the Game manager, which is then passed to the car mode
    /// </summary>
    public uint ordersReadyToDeliver;
    /// <summary>
    /// This would be the total number of orders for the day the player needs to complete before they can leave the Pizzaria
    /// </summary>
    public uint ordersRequired;

    public Transform ambianceSoundLocation;
    public bool ReadyToLeave()
    {
        return ordersReadyToDeliver >= ordersRequired;
    }
}
