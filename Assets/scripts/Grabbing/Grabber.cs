using UnityEngine;
namespace Grabbing
{
    public class Grabber: MonoBehaviour
    {
        private Grabbable currentlyGrabbed;
        public Vector3 grabbableOffset;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.TransformVector(grabbableOffset), 0.1f);
        }
    }
}