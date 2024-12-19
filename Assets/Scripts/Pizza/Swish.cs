using Grabbing;
using UnityEngine;
using UnityEngine.Events;

public class Swish : MonoBehaviour
{
	public UnityEvent<GameObject> onObjectScored;
    public UnityEvent<GameObject> on3ptScored;

    public PizzaModeManager modeManager;
	public float threePointDistance = 50f;
    private void Start()
    {
		modeManager = PizzaModeManager.singleton;
    }
    public void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Grabbable>() != null)
		{
			if (Vector3.Distance(transform.position, modeManager.playerMaster.transform.position) >= threePointDistance)
			{
                on3ptScored.Invoke(other.gameObject);
				AcheivementManager.UnlockAchievement("THREE_POINTER");
            }
            onObjectScored.Invoke(other.gameObject);
		}
	}
}
