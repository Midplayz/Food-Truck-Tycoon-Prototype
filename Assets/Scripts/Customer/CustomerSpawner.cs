using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    private GameObject currentCustomer;
    public float minSpawnDelay = 2f;
    public float maxSpawnDelay = 5f;

    void OnEnable()
    {
        CustomerBehavior.OnCustomerLeft += HandleCustomerLeft;
    }

    void OnDisable()
    {
        CustomerBehavior.OnCustomerLeft -= HandleCustomerLeft;
    }

    public void StartSpawningCustomers()
    {
        StartCoroutine(WaitAndSpawnCustomer());
    }

    public void StopSpawningCustomers()
    {
        StopCoroutine(WaitAndSpawnCustomer());
    }

    void SpawnCustomer()
    {
        if (GameLoop.instance.ShouldStopSpawning())
        {
            return;
        }

        currentCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void HandleCustomerLeft(bool satisfied, int income)
    {
        Debug.Log($"Customer left. Satisfied: {satisfied}, Income: {income}");
        currentCustomer = null;

        StartCoroutine(WaitAndSpawnCustomer());
    }

    IEnumerator WaitAndSpawnCustomer()
    {
        float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(waitTime);

        if (!GameLoop.instance.ShouldStopSpawning())
        {
            SpawnCustomer();
        }
    }
}
