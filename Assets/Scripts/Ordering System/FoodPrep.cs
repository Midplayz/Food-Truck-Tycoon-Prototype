using UnityEngine;
using System.Collections;
using TMPro;

public class FoodPrep : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TextMeshProUGUI informationalText;
    private GameObject currentSpawnedFood;
    private Coroutine prepCoroutine;

    public void PrepFood()
    {
        if (OrderingSystem.Instance == null || OrderingSystem.Instance.GetCurrentOrderCount() == 0)
        {
            informationalText.text = "No Orders That Can Be Prepared!";
            Debug.Log("No orders to prepare!");
            return;
        }

        GameObject firstOrder = OrderingSystem.Instance.GetFirstOrder();
        if (firstOrder == null) return;

        OrderDetails orderDetails = firstOrder.GetComponent<OrderDetails>();
        if (orderDetails == null)
        {
            Debug.LogError("OrderDetails component missing!");
            return;
        }

        MenuItem menuItem = orderDetails.assignedMenuItem;
        Debug.Log($"Preparing {menuItem.name} for order.");
        if (menuItem == null || menuItem.itemPrefab == null)
        {
            Debug.LogError("Invalid MenuItem data!");
            return;
        }

        if (currentSpawnedFood != null)
        {
            Debug.Log("Food prep station is occupied!");
            return;
        }

        if (prepCoroutine != null)
        {
            StopCoroutine(prepCoroutine);
            Debug.Log("Previous food prep interrupted!");
        }

        prepCoroutine = StartCoroutine(CookFood(menuItem));
    }

    private IEnumerator CookFood(MenuItem menuItem)
    {
        Debug.Log($"Preparing {menuItem.name}. Time: {menuItem.cookTime}s");
        informationalText.text = $"Preparing {menuItem.name}. Time: {menuItem.cookTime}s";

        yield return new WaitForSeconds(menuItem.cookTime);

        if (currentSpawnedFood != null)
        {
            Debug.Log("Food station still occupied after waiting!");
            yield break;
        }

        InventoryManager.Instance.UseIngredients(menuItem.ingredients);
        currentSpawnedFood = Instantiate(menuItem.itemPrefab, spawnPoint.position, Quaternion.identity);
        SpawnedPrefab spawnedPrefab = currentSpawnedFood.GetComponent<SpawnedPrefab>();
        spawnedPrefab.menuItem = menuItem;
        informationalText.text = $"{menuItem.name} is ready!";
        Debug.Log($"{menuItem.name} is ready!");
    }

    public void InterruptPrep()
    {
        if (prepCoroutine != null)
        {
            StopCoroutine(prepCoroutine);
            prepCoroutine = null;
            Debug.Log("Food preparation interrupted!");
        }

        if (currentSpawnedFood != null)
        {
            Destroy(currentSpawnedFood);
            currentSpawnedFood = null;
            Debug.Log("Previous food removed from station.");
        }
    }

    public void ClearCurrentItem()
    {
        if (currentSpawnedFood != null)
        {
            currentSpawnedFood = null;
        }
    }
}
