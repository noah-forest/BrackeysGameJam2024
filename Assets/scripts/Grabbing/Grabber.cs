using UnityEngine;
namespace Grabbing
{
    public class Grabber: MonoBehaviour
    {
        private Grabbable currentlyGrabbed;
        public Vector3 grabbableOffset;
    
        public void Grab(Grabbable grabbable)
        {
            if (currentlyGrabbed != null)
            {
                currentlyGrabbed.Release();
            }
            currentlyGrabbed = grabbable;
            currentlyGrabbed.transform.SetParent(this.transform);
            currentlyGrabbed.transform.localPosition = grabbableOffset + currentlyGrabbed.offset;
            currentlyGrabbed.Grab();
        }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.TransformVector(grabbableOffset), 0.1f);
        }
    }
}