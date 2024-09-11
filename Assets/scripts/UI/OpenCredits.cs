using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCredits : MonoBehaviour
{
    public GameObject menu;
    public GameObject close;
    private bool open;
    private Button button;
    
    private void Start()
    {
        button = GetComponent<Button>();
        
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (open)
        {
            open = false; 
            close.SetActive(false);
            menu.SetActive(false);
        }
        else
        {
            open = true;
            close.SetActive(true);
            menu.SetActive(true);
        }
    }
}
