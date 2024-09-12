using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Interact;
using UnityEngine.Events;

namespace Interact
{
    /// <summary>
    /// can Be used if you want the range in which you can interact with the interactable to be less than the players raycast distance
    /// </summary>
    public class ReducedRangeInteractable : PizzaModeInteractable
    {
        public float range = 2f;

        // Start is called before the first frame update
        public override bool CanInteract()
        {
            //if (Vector3.Distance(gameObject.transform.position, transform.position) <= range)
            //{
            //    return true;
            //}

            //return false;
            return true;
        }

        public override void OnLook()
        {
            //if (Vector3.Distance(gameObject.transform.position, transform.position) > range) return;
            base.OnLook();

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}