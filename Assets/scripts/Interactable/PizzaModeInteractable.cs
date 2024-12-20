﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interact
{
    public class PizzaModeInteractable : MonoBehaviour, IInteractable
    {
        public UnityEvent<GameObject> onInteract;
        [SerializeField] GameObject HoverDisplay;
        public string displayText;
        public virtual bool CanInteract(GameObject interactor)
        { 
            return true;
        }

        public virtual string GetDisplayString()
        {
            return displayText;
        }

        public virtual void Interact(GameObject gameObject)
        {
            this.onInteract.Invoke(gameObject);
        }

        public virtual void OnLook()
        {
            
        }

        public virtual void OnLookAway()
        {
            
        }
    }
}