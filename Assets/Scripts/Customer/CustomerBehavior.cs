using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomerBehavior : MonoBehaviour
{
    public string customerOrder;
    public Dictionary<string, int> requiredIngredients = new Dictionary<string, int>();
    public float patience = 20f;
    public int orderValue = 0;

    [Header("Patience Bar")]
    public Image patienceBar;
    public Canvas canvas;

    private float timer;
    private bool served = false;

    public delegate void CustomerLeftEvent(bool satisfied, int income);
    public static event CustomerLeftEvent OnCustomerLeft;

    private Dictionary<string, int> dishSellingPrices = new Dictionary<string, int>()
    {
        { "Sushi", 18 },
        { "Sashimi", 17 },
        { "Nigiri", 12 },
        { "Maki", 16 },
        { "Onigiri", 15 }
    };

    private Dictionary<string, Dictionary<string, int>> dishes = new Dictionary<string, Dictionary<string, int>>()
    {
        { "Sushi", new Dictionary<string, int> { { "Rice", 2 }, { "Fish", 1 }, { "Soy", 1 } } },
        { "Sashimi", new Dictionary<string, int> { { "Fish", 2 }, { "Soy", 1 } } },
        { "Nigiri", new Dictionary<string, int> { { "Rice", 1 }, { "Fish", 1 } } },
        { "Maki", new Dictionary<string, int> { { "Rice", 2 }, { "Cucumber", 1 }, { "Sesame", 1 } } },
        { "Onigiri", new Dictionary<string, int> { { "Rice", 3 }, { "Sesame", 1 } } }
    };

    void Start()
    {
        GenerateOrder();
        timer = patience;

        if (patienceBar != null)
        {
            patienceBar.fillAmount = 1.0f;
            patienceBar.color = Color.green;
        }
    }

    void Update()
    {
        if (!served)
        {
            timer -= Time.deltaTime;

            if (patienceBar != null)
            {
                patienceBar.fillAmount = Mathf.Clamp(timer / patience, 0, 1);

                patienceBar.color = Color.Lerp(Color.red, Color.green, timer / patience);
            }

            if (timer <= 0)
            {
                LeaveTruck(false);
            }
        }

        if (canvas != null)
        {
            canvas.transform.LookAt(Camera.main.transform);
            canvas.transform.Rotate(0, 180, 0); 
        }
    }

    void GenerateOrder()
    {
        List<string> dishNames = new List<string>(dishes.Keys);
        string selectedDish = dishNames[Random.Range(0, dishNames.Count)];

        requiredIngredients = new Dictionary<string, int>(dishes[selectedDish]);
        customerOrder = $"Dish: {selectedDish}";

        if (dishSellingPrices.ContainsKey(selectedDish))
        {
            orderValue = dishSellingPrices[selectedDish];
        }
        else
        {
            Debug.LogError($"No selling price defined for {selectedDish}");
        }

        Debug.Log($"Customer ordered {selectedDish} (Selling Price: {orderValue}) with ingredients:");
        foreach (var ingredient in requiredIngredients)
        {
            Debug.Log($"{ingredient.Key}: {ingredient.Value}");
        }
    }

    public void FulfillOrder(InventoryManager inventoryManager)
    {
        if (inventoryManager.UseIngredients(requiredIngredients))
        {
            served = true;
            LeaveTruck(true);
        }
        else
        {
            Debug.Log("Order could not be fulfilled.");
        }
    }

    void LeaveTruck(bool satisfied)
    {
        OnCustomerLeft?.Invoke(satisfied, satisfied ? orderValue : 0);
        Destroy(gameObject);
    }
}
