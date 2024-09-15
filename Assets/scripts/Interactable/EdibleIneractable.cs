using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interact;
using Grabbing;
public class EdibleIneractable : Grabbable
{
    [SerializeField] protected int maxUses;
    public int Uses { get; private set; }
    [SerializeField] Rigidbody rb;

    public virtual void DeductUse()
    {
        if (!CanEat()) return;
        ++Uses;
        rb.mass /= Uses;
    }

    public virtual bool CanEat()
    {
        if (Uses < maxUses) return true;
        return false;
    }
}
