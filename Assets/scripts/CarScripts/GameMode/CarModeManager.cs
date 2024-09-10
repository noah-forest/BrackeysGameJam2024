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
    [SerializeField] LayerMask goundMask;

    [SerializeField] float horizontalOffset;
    [SerializeField] float verticalOffset;
    [SerializeField] Transform homeBase;

    List<BoxCollider> possibleRoads = new List<BoxCollider>();

    [SerializeField] float deliveryReward;

    /*[HideInInspector]*/ public uint pizzasToDeliver = 10;
    [HideInInspector] public float moneyEarned;

    [SerializeField] CarGoal goalInstance;
    GameObject currentBuilding;
    float timeRemaining;

    public void DeliverPizza()
    {
        if (pizzasToDeliver != 0)
        {
            pizzasToDeliver--;
            moneyEarned = (float)System.Math.Round((double)(moneyEarned + deliveryReward), 2);
        }
        UpdateGoal();
    }
    public void LosePizza()
    {
        if (pizzasToDeliver != 0)
        {
            pizzasToDeliver--;
        }
    }

    private void Start()
    {
        possibleRoads.AddRange(goalSpawners);
        Debug.Log($"[CAR MODE MANAGER][Start] Pizzas to Deliver : {pizzasToDeliver}");
        UpdateGoal();
        car.health.carDamaged.AddListener(LosePizza);
    }

    public bool TryGenerateNextGoal()
    {
        BoxCollider street = possibleRoads[Random.Range(0, possibleRoads.Count)];
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
            Vector3 goalPos = hit.point + hit.normal * horizontalOffset;
            goalPos.y = verticalOffset;
            goalInstance.transform.position = goalPos;
            goalInstance.transform.forward = hit.normal;
            if (!Physics.Raycast(goalInstance.transform.position, Vector3.down, out hit, buildingDetectionRadius, goundMask, QueryTriggerInteraction.Ignore))
            {
                return false;
            }

        }
        else
        {
            return false;
        }

        possibleRoads.Remove(street);
        return true;
    }


    private void UpdateGoal()
    {
        if(!goalInstance)
        {
            Debug.Log("[CAR MODE MANAGER][UPDATE GOAL] No Goal Assigned");
            return;
        }
        if (pizzasToDeliver > 0)
        {
            if(possibleRoads.Count == 0)
            {
                possibleRoads.AddRange(goalSpawners);
            }
            while (!TryGenerateNextGoal()) ;
        }
        else
        {
            goalInstance.transform.position = homeBase.position;
            goalInstance.transform.rotation = homeBase.rotation;
        }
    }
}
