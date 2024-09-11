using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interact;
using Grabbing;
public class EdibleIneractable : Grabbable
{
    [SerializeField] protected int usesRemaining;

    public virtual void DeductUse()
    {
        --usesRemaining;
    }

    public virtual bool CanEat()
    {
        if (usesRemaining > 0) return true;
        return false;
    }
}
