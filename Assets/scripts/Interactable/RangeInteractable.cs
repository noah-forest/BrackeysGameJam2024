using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Interact;

namespace Interact
{
    public abstract class RangeInteractable : MonoBehaviour, IInteractable
    {
        public float range = 2f;

        // Start is called before the first frame update
        public bool CanInteract(GameObject gameObject)
        {
            if (Vector3.Distance(gameObject.transform.position, transform.position) < range)
            {
                return true;
            }

            return false;
        }

        public abstract void Interact(GameObject gameObject);

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}