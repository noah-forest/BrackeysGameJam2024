using UnityEngine;

public class Cigarette : MonoBehaviour
{
	public Mesh[] cigStages;

	public ParticleSystem smokeParticles;
	public ParticleSystem emberParticles;
	public Transform emberPositioner;
	public MeshCollider[] cigColliders = new MeshCollider[3];

	public bool smokeable;

	public GameObject leftArm;

	private MeshFilter filter;
	private int currentCigIndex;
	private int cigCount;

	private void Start()
	{
		filter = GetComponent<MeshFilter>();
		InitializeCig();
	}

	public void Smoke()
	{
		++currentCigIndex;

		var meshInterp = (int)Mathf.Lerp(1, cigStages.Length, (float)currentCigIndex / cigCount);

		if (currentCigIndex >= cigCount)
		{
			meshInterp = 0;
			smokeable = false;
			leftArm.SetActive(false);
		}

		filter.mesh = cigStages[meshInterp];

		if (meshInterp is 2 or 3)
		{
			transform.localPosition = new Vector3(-0.458000004f, 0.959999979f, 0.214000002f);
		}

		for (int i = 0; i < cigColliders.Length; i++)
		{
			if (i == meshInterp)
			{
				cigColliders[i].enabled = true;
			}
			else
			{
				cigColliders[i].enabled = false;
			}
		}

		RaycastHit hit;
		if (Physics.Raycast(emberPositioner.position, emberPositioner.forward * -1, out hit, 1000, LayerMask.GetMask("Cig")))
		{
			if (hit.collider)
			{
				emberParticles.gameObject.transform.position = hit.point;
			}
		}
	}

	public void InitializeCig()
	{
		cigCount = cigStages.Length;
		smokeable = true;
		currentCigIndex = 0;
		filter.mesh = cigStages[0];
	}
}
