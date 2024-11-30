using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBottle : MonoBehaviour
{
    [SerializeField] GameObject[] pieces;
    [SerializeField] float velMultiplier = 2f;
    [SerializeField] float timeBeforeDestroying = 60f;

    static List<GameObject> allBottlePieces = new List<GameObject>();
    static int overalPeiceLimit = 12;

    void Start()
    {
        Destroy(this.gameObject, timeBeforeDestroying);
        allBottlePieces.RemoveAll((x) => x == null); 
        for (int i = 0; i < pieces.Length; i++)
        {
            allBottlePieces.Add(pieces[i]);
            if (allBottlePieces.Count > overalPeiceLimit)
            {
                //Debug.Log(overalPeiceLimit + " Peices Exceeded. Destroying oldest : " + allBottlePieces[0]?.name + "|");

                Destroy(allBottlePieces[0]);
                allBottlePieces.RemoveAt(0);
            }
        }
        //Debug.Log(allBottlePieces.Count);
    }
    
    public void RandomVelocities()
    {
        for(int i = 0; i <= pieces.Length - 1; i++)
        {
            float xVel = UnityEngine.Random.Range(0f, 1f);
            float yVel = UnityEngine.Random.Range(0f, 1f);
            float zVel = UnityEngine.Random.Range(0f, 1f);
            Vector3 vel = new Vector3(velMultiplier * xVel, velMultiplier * yVel, velMultiplier * zVel);
            pieces[i].GetComponent<Rigidbody>().velocity = vel;


        }
    }
}