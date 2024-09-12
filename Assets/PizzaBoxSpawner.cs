using Grabbing;
using PizzaOrder;
using UnityEngine;

public class PizzaBoxSpawner : MonoBehaviour
{
    public PizzaBox pizzaBoxPrefab;
    private bool expectingPizzaBox = false;
    PizzaBox currentPizzaBox;
    Grabbable grabbable;

    private float timeSpentStill = 0;
    private Vector3 lastPosition;

    public Order currentOrder
    {
        get => _currentOrder;
    }
    private Order _currentOrder;
    
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    public void SetCurrentOrder(Order order)
    {
        _currentOrder = order;
        // SpawnPizzaBox();
    }

    public void ClearOrder()
    {
        _currentOrder = null;
        expectingPizzaBox = false;
    }
    
    private void FixedUpdate()
    {
        if (currentOrder != null && currentOrder.IsCompleted())
        {
            ClearOrder();
            DestroyCurrentBox();
        }
        if (currentPizzaBox == null && currentOrder != null && !currentOrder.IsCompleted())
        {
            SpawnPizzaBox();
        }
        if (currentPizzaBox != null && grabbable != null && grabbable.isGrabbed == false)
        {
            var currentPosition = currentPizzaBox.transform.position;
            if (Vector3.Distance(lastPosition, currentPosition) > 0.05f && !BySpawn())
            {
                timeSpentStill += Time.deltaTime;
            }
            else
            {
                timeSpentStill = 0;
            }

            if (timeSpentStill > 15)
            {
                SpawnPizzaBox();
                // currentPizzaBox.transform.position = transform.position;
                timeSpentStill = 0;
            }
        }
    }

    void DestroyCurrentBox()
    {
        if (currentPizzaBox != null)
        {
            Destroy(currentPizzaBox.gameObject);
        }
    }

    bool BySpawn()
    {
        return Vector3.Distance(transform.position, currentPizzaBox.transform.position) < 0.3f;
    }
    
    void SpawnPizzaBox()
    {
        DestroyCurrentBox();
        expectingPizzaBox = true;
        currentPizzaBox = Instantiate(pizzaBoxPrefab, transform.position, transform.rotation);
        currentPizzaBox.SetOrder(currentOrder);
        grabbable = currentPizzaBox.GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(() =>
        {
            timeSpentStill = 0;
        });
    }
}
