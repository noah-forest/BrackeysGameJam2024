using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenSideMenu : MonoBehaviour
{
    public GameObject menu;
    private bool open;

    public void OnClick()
    {
        if (open)
        {
            open = false; 
            menu.SetActive(false);
        }
        else
        {
            open = true;
            menu.SetActive(true);
        }
    }
}
