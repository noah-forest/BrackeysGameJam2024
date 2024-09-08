using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Interact
{

    public class Interactor : MonoBehaviour
    {
        void Update()
        {
            // on mouse down
            if (Input.GetMouseButtonDown(0))
            {
                // raycast from camera
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    RangeInteractable grabbableHitbox = hit.collider.gameObject.GetComponent<RangeInteractable>();
                    if (grabbableHitbox != null && grabbableHitbox.CanInteract(this.gameObject))
                    {
                        grabbableHitbox.Interact(this.gameObject);
                    }
                }
            }
        }
    }

}