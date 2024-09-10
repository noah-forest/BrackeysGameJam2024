using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModeManager : MonoBehaviour
{
    #region singleton

    public static CarModeManager singleton;

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


    public int pizzasToDeliver;

}
