using Grabbing;
using UnityEngine;
using UnityEngine.Events;

public class Swish : MonoBehaviour
{
	public UnityEvent<GameObject> onObjectScored;

	public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Grabbable>() != null)
		{
			onObjectScored.Invoke(other.gameObject);
		}
	}
}
