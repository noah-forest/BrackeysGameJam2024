using Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvRemoteInteractable : RangeInteractable
{

    [SerializeField] SelectRandomVideo tv;
    [SerializeField] GameObject ScreenImage;
    // Start is called before the first frame update

    public override void Interact(GameObject gameObject)
    {
        base.Interact(gameObject);
        tv?.TogglePower();
        ScreenImage?.SetActive(!(bool)ScreenImage?.activeSelf);
    }
}
