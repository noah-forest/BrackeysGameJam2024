using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnSetDay : MonoBehaviour
{
    [SerializeField] int day;
    [SerializeField] bool enableOnGreaterThan = true;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.singleton;
        if((day != gameManager.Day) || (enableOnGreaterThan && day <= gameManager.Day))
        {
            gameObject.SetActive(false);
        }
    }

}
