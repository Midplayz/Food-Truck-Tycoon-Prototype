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
    private float orderRateMultiplier = 1f; 
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartOrdering();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StopOrdering();
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

            float waitTime = Random.Range(minOrderTime, maxOrderTime) / orderRateMultiplier;
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

    public void IncreaseOrderRate(float multiplier)
    {
        orderRateMultiplier = Mathf.Max(0.1f, orderRateMultiplier * multiplier);
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

}
