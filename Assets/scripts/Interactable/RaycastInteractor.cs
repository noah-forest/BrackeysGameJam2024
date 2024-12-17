using Grabbing;
using UnityEngine;
using UnityEngine.Events;

namespace Interact
{

    public class RaycastInteractor : MonoBehaviour
    {
        public UnityEvent<GameObject> onInteract = new();
        public UnityEvent onInput = new();

        public UnityEvent<GameObject, string> onLook = new();
        public UnityEvent<GameObject, string> onLookAway = new();

        public LayerMask layerMask;
        [SerializeField] Transform head;
        [SerializeField] float distance;

        IInteractable targetInteractable;

        GameObject objUnderCrosshair;

        public Grabber Grabber { get; private set; }

        void Awake()
        {
            //onLook.AddListener(TestOnLook);
            //onLookAway.AddListener(TestOnLookAway);
            Grabber = GetComponentInChildren<Grabber>();

        }
        void LateUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(head.position, head.forward, out hit, distance, layerMask))
            {
                GameObject newObj = hit.collider.gameObject;
                if (objUnderCrosshair != newObj)
                {
                    if (newObj) SetTargetInteractor(newObj);
                    objUnderCrosshair = newObj;
                }
            }
            else if (objUnderCrosshair)
            {
                SetTargetInteractor(null);
                objUnderCrosshair = null;
            }

            // on mouse down
            if (Input.GetMouseButtonDown(0) && !GameManager.singleton.UIManager.gameIsPaused)
            {
                onInput.Invoke();
                // raycast from camera
                if (targetInteractable != null && targetInteractable.CanInteract(gameObject))
                {
                    //EdibleIneractable drink = Grabber.currentlyGrabbed?.GetComponent<EdibleIneractable>();
                    //if (drink && drink.Uses > 0)
                    //{
                    //    return;
                    //}

                    targetInteractable.Interact(gameObject);

                    onInteract.Invoke(objUnderCrosshair);
                }
            }
        }

        void TestOnLook(GameObject obj, string displaytext)
        {
            if (obj) Debug.Log("LookedAt from:" + obj.name + displaytext);
        }
        void TestOnLookAway(GameObject obj, string displaytext)
        {
            if (obj) Debug.Log("LookedAway from:" + obj.name + displaytext);
        }


        void SetTargetInteractor(GameObject obj)
        {
            IInteractable interactable = obj?.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract(gameObject))
            {
                targetInteractable = interactable;
                onLook.Invoke(objUnderCrosshair, interactable.GetDisplayString());
            }
            else
            {
                onLookAway.Invoke(objUnderCrosshair, interactable?.GetDisplayString());
                targetInteractable = null;
            }
        }

    }
}