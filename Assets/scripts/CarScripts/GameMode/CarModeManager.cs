using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarModeManager : MonoBehaviour
{
    #region singleton

    public static CarModeManager singleton;


    private void Awake()
    {
        if (singleton)
        {
            Destroy(this.gameObject);
            return;
        }

        singleton = this;
        car.modeManager = this;
    }
    #endregion

    public CarMaster car;
    public GameManager gameManager;
    [SerializeField] BoxCollider[] goalSpawners;
    [SerializeField] float buildingDetectionRadius = 50;
    [SerializeField] LayerMask buildingMask;
    [SerializeField] LayerMask goundMask;

    public Transform ambianceSoundLocation;
    [SerializeField] float horizontalOffset;
    [SerializeField] float verticalOffset;
    [SerializeField] Transform homeBase;

    List<BoxCollider> possibleRoads = new List<BoxCollider>();

    [SerializeField] float deliveryReward;

    /*[HideInInspector]*/ public uint _pizzasToDeliver = 10;
    [HideInInspector] public UnityEvent<uint> pizzasChanged = new();
    bool atHomeBase;
    public uint PizzasToDeliver
    {
        get 
        { 
            return _pizzasToDeliver; 
        }
        set
        {
            _pizzasToDeliver = value;
            pizzasChanged.Invoke(_pizzasToDeliver);
        }
    }
    [HideInInspector] public float moneyEarned;

    [SerializeField] CarGoal goalInstance;
    GameObject currentBuilding;
    float timeRemaining;

    CarModeMusic musicPlayer;

    public void DeliverPizza()
    {
        if (PizzasToDeliver != 0)
        {
            PizzasToDeliver--;
            moneyEarned = (float)System.Math.Round((double)(moneyEarned + deliveryReward), 2);
        }
        UpdateGoal();
    }
    public void LosePizza()
    {
        if (PizzasToDeliver != 0)
        {
            PizzasToDeliver--;
            gameManager.UnscoreNextPizza();
            if(PizzasToDeliver == 0) UpdateGoal();
        }
    }

    private void Start()
    {
        possibleRoads.AddRange(goalSpawners);
        //Debug.Log($"[CAR MODE MANAGER][Start] Pizzas to Deliver : {PizzasToDeliver}");
        UpdateGoal();
        car.health.carDamaged.AddListener(LosePizza);
        pizzasChanged.AddListener(LogPizzaCount);
    }

    void LogPizzaCount(uint count)
    {
        Debug.Log("Pizzas Remaining = " + count);
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

        if (atHomeBase)
        {
            Destroy(goalInstance.gameObject);
            StartCoroutine(StartPizza());
            return;
        }

        if (PizzasToDeliver > 0)
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
            StartCoroutine(SetHomeBase());
        }
        car.compass.currentGoal = goalInstance.transform;
    }
    
    IEnumerator StartPizza()
    {
        yield return new WaitForSeconds(2);
        gameManager.LoadDayOver();
    }
    IEnumerator SetHomeBase() // this exists to prevent frame issue/ occasioan instant scene change on last delivery
    {
        yield return new WaitForSeconds(0.1f);
        atHomeBase = true;
    }
}
