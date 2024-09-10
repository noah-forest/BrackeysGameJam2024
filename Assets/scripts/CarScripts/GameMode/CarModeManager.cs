using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModeManager : MonoBehaviour
{
    #region singleton

    public static CarModeManager singleton;
    public CarMaster car;

    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }

        singleton = this;
    }
    #endregion


    [SerializeField] BoxCollider[] goalSpawners;
    [SerializeField] float buildingDetectionRadius = 50;
    [SerializeField] LayerMask buildingMask;
    [SerializeField] GameObject goalPrefab;
    [SerializeField] float sideWalkSize;

    HashSet<BoxCollider> alreadyChosen;

    [SerializeField] float deliveryReward;

    [HideInInspector] public uint pizzasToDeliver;
    [HideInInspector] public float money;

    CarGoal goalInstance;
    GameObject currentBuilding;
    float timeRemaining;

    public void DeliverPizza()
    {
        if (pizzasToDeliver != 0)
        {
            pizzasToDeliver--;
            money = (float)System.Math.Round((double)(money + deliveryReward), 2);
        }
        else
        {
            // return to pizza ?
        }
    }

    private void Start()
    {
        //goalInstance = Instantiate(goalPrefab, transform).GetComponent<CarGoal>();
        //while (TryGenerateNextGoal());
    }

    public bool TryGenerateNextGoal()
    {
        BoxCollider street = goalSpawners[Random.Range(0, goalSpawners.Length)];
        float xBound = Random.Range(street.bounds.min.x, street.bounds.max.x);
        float zBound = Random.Range(street.bounds.min.z, street.bounds.max.z);
        Vector3 point = new Vector3(xBound, 1, zBound);

        Collider[] buildingsInRadius = Physics.OverlapSphere(point, buildingDetectionRadius, buildingMask);
        if (buildingsInRadius.Length == 0) return false;
        Collider closest = buildingsInRadius[0];
        float bestDistance = Vector3.Distance(point, closest.transform.position);
        foreach (Collider bld in buildingsInRadius)
        {
            if (Vector3.Distance(point, bld.transform.position) < bestDistance) closest = bld;
        }

        Vector3 dir = (closest.transform.position - point).normalized;

        RaycastHit hit;
        if (Physics.Raycast(point, dir, out hit, buildingDetectionRadius, buildingMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 goalPos = hit.point + hit.normal * (sideWalkSize + goalInstance.boundingBox.size.x * 0.5f);
            goalInstance.transform.position = goalPos;
        }
        else
        {
            return false;
        }

        return true;
    }

}
