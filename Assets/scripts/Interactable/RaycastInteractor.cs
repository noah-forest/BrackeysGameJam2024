using UnityEngine;
using UnityEngine.Events;

namespace Interact
{

    public class RaycastInteractor : MonoBehaviour
    {
        public UnityEvent<GameObject> onInteract = new();

        public LayerMask layerMask;
        [SerializeField] Transform head;
        [SerializeField] float distance;

        IInteractable _target;
        public IInteractable Target 
        {
            get { return _target; }
            set
            {
                if (_target != null) // call look away on old target if there was one
                {
                    _target.OnLookAway();
                }

                _target = value;    // set new target
                if(_target != null) // call look if we have one
                {
                    _target.OnLook();
                }
                else
                {
                }
            }


        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distance, layerMask))
            {
                Target = hit.collider.gameObject.GetComponent<IInteractable>();
            }
            else
            {
                Target = null;
            }

            // on mouse down
            if (Input.GetMouseButtonDown(0))
            {
                // raycast from camera
                if (Target != null)
                {
                    Target.Interact(this.gameObject);
                    onInteract.Invoke(hit.collider.gameObject);
                }
            }
        }
    }

}