using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interact;
using Grabbing;
public class EdibleIneractable : Grabbable
{
    [SerializeField] protected int maxUses;
    int uses;
    [SerializeField] Rigidbody rb;

    public virtual void DeductUse()
    {
        if (!CanEat()) return;
        ++uses;
        rb.mass /= uses;
    }

    public virtual bool CanEat()
    {
        if (uses < maxUses) return true;
        return false;
    }
}
