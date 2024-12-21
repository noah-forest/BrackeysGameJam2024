using System.Collections.Generic;
using UnityEngine;

public class SpawnTurrets : MonoBehaviour
{
	public GameObject turret;
	public List<GameObject> spawns;

	private GameManager gameManager;
	private CarModeManager carModeManager;

	[SerializeField] private int numToSpawn = 3;

	private void Start()
	{
		gameManager = GameManager.singleton;
		carModeManager = CarModeManager.singleton;

		spawns = new List<GameObject>(spawns);

		GenerateSpawnList();
		SpawnTurretsAtLoc();
		RampTurretSpawns();
	}

	private void SpawnTurretsAtLoc()
	{
		for (int i = 0; i < numToSpawn; i++)
		{
			var randomPoint = Random.Range(0, spawns.Count);
			var temp = spawns[randomPoint];

			var obj = Instantiate(turret, temp.transform);
			obj.transform.SetPositionAndRotation(temp.transform.position, temp.transform.rotation);

			spawns.RemoveAt(randomPoint);

			//maximum score you can get if you destroy all possible turrets
			carModeManager.possibleTurretScore += carModeManager.gainedFromKill;
		}
	}

	private void GenerateSpawnList()
	{
		Transform[] childTransforms = transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in childTransforms)
		{
			spawns.Add(child.gameObject);
		}
		spawns.RemoveAt(0); //this is fucking dumb
	}

	private void RampTurretSpawns()
	{
		if (gameManager.Day != 1)
		{
			numToSpawn += 2; //this will spawn 2 extra turrets a day
		}
	}
}
