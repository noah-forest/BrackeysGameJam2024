using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnSetDay : MonoBehaviour
{
    [SerializeField] int day;
    [SerializeField] bool enableOnGreaterThan = true;
    [SerializeField] bool debug;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.singleton;
        if (debug) Debug.Log($"[ENABLEONDAY]{gameObject.name} Checking day. cur day: {gameManager.Day} dayNeeded {day} ");
        if((day != gameManager.Day) || (enableOnGreaterThan && day < gameManager.Day))
        {
            gameObject.SetActive(false);
        }
    }

}
