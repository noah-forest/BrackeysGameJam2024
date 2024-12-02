using Grabbing;
using UnityEngine;
public class EdibleIneractable : Grabbable
{
	public Mesh[] beerStages;
	public MeshFilter filter;

	[SerializeField] protected int maxUses;
	public int Uses { get; private set; }
	[SerializeField] Rigidbody rb;

	private int beerAmt;
	private int currentBeerIndex;

	private void Awake()
	{
		InitializeBeer();
	}

	public virtual void DeductUse()
	{
		if (!CanEat()) return;

		++currentBeerIndex;

		var meshInterp = (int)Mathf.Lerp(1, beerStages.Length, (float)currentBeerIndex / beerAmt);

		if (currentBeerIndex >= beerAmt)
		{
			meshInterp = 0;
		}

		filter.mesh = beerStages[meshInterp];

		++Uses;
		rb.mass /= Uses;
	}

	public virtual bool CanEat()
	{
		if (Uses < maxUses) return true;
		return false;
	}

	public void InitializeBeer()
	{
		beerAmt = beerStages.Length;
		currentBeerIndex = 0;
		filter.mesh = beerStages[0];
	}
}
