using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrderingSystem : MonoBehaviour
{
    public static OrderingSystem Instance;

    [Header("Order Settings")]
    [SerializeField] private GameObject orderPrefab; 
    [SerializeField] private Transform orderParent; 
    [SerializeField] private float minOrderTime = 5f; 
    [SerializeField] private float maxOrderTime = 15f; 
    [SerializeField] private int maxCurrentOrders = 5; 

    private List<GameObject> activeOrders = new List<GameObject>(); 
    private bool canOrder = false; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartOrdering()
    {
        if (!canOrder)
        {
            canOrder = true;
            StartCoroutine(SpawnOrders());
        }
    }

    public void StopOrdering()
    {
        canOrder = false;
        StopAllCoroutines(); 
    }

    private IEnumerator SpawnOrders()
    {
        while (canOrder)
        {
            if (activeOrders.Count < maxCurrentOrders)
            {
                SpawnOrder();
            }

            float waitTime = Random.Range(minOrderTime, maxOrderTime) / PreDayPrep.Instance.customerSpawnRateMultipler;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnOrder()
    {
        GameObject newOrder = Instantiate(orderPrefab, orderParent);
        activeOrders.Add(newOrder);

        newOrder.GetComponent<OrderDetails>().StartCoroutine(TrackOrderLifetime(newOrder));
    }

    private IEnumerator TrackOrderLifetime(GameObject order)
    {
        yield return new WaitUntil(() => order == null);
        activeOrders.Remove(order);
    }

    public void SetMaxOrders(int maxOrders)
    {
        maxCurrentOrders = maxOrders;
    }

    public int GetCurrentOrderCount()
    {
        return activeOrders.Count;
    }
    public void RemoveOrder(GameObject order)
    {
        if (activeOrders.Contains(order))
        {
            activeOrders.Remove(order);
        }
    }

    public void FulfillOrder(MenuItem servedItem)
    {
        OrderDetails orderToServe = null;
        float minTimeLeft = float.MaxValue;

        foreach (GameObject orderObj in activeOrders)
        {
            OrderDetails orderDetails = orderObj.GetComponent<OrderDetails>();
            if (orderDetails != null && orderDetails.assignedMenuItem.name == servedItem.name)
            {
                if (orderDetails.GetRemainingTime() < minTimeLeft)
                {
                    minTimeLeft = orderDetails.GetRemainingTime();
                    orderToServe = orderDetails;
                }
            }
        }

        if (orderToServe != null)
        {
            GameManager.instance.RegisterOrderFulfillment(true, orderToServe.priceValue);
            activeOrders.Remove(orderToServe.gameObject);
            Destroy(orderToServe.gameObject);
            Debug.Log($"Order for {servedItem.name} fulfilled!");
        }
        else
        {
            Debug.Log("Not Needed!");
        }
    }

    public GameObject GetFirstOrder()
    {
        foreach (GameObject order in activeOrders)
        {
            OrderDetails orderDetails = order.GetComponent<OrderDetails>();

            if (orderDetails != null)
            {
                float cookTimeWithBuffer = orderDetails.assignedMenuItem.cookTime + 3;
                float remainingTime = orderDetails.GetRemainingTime();

                if (cookTimeWithBuffer < remainingTime)
                {
                    if (InventoryManager.Instance.HasEnoughIngredients(orderDetails.assignedMenuItem.ingredients))
                    {
                        return order;
                    }
                }
            }
        }

        return null;
    }
}
