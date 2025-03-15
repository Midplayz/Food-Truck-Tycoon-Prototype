using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private Dictionary<string, int> ingredientStock = new Dictionary<string, int>();

    [Header("UI Elements")]
    public Transform inventoryContent; 
    public GameObject ingredientPrefab;
    public GameObject inventoryPanel;

    private Dictionary<string, GameObject> ingredientUIElements = new Dictionary<string, GameObject>();

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

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if(inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            MovementValues.Instance.ToggleMovementCompletely(true);
            inventoryPanel.SetActive(false);
            Debug.Log("tru");

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            MovementValues.Instance.ToggleMovementCompletely(false);
            inventoryPanel.SetActive(true);
            Debug.Log("else");

        }
    }

    public void InitializeInventory(List<IngredientItem> selectedIngredients)
    {
        if (selectedIngredients == null || selectedIngredients.Count == 0)
        {
            Debug.LogError("InventoryManager: No ingredients provided for initialization!");
            return;
        }

        ingredientStock.Clear();
        ClearUI();

        foreach (var ingredient in selectedIngredients)
        {
            ingredientStock[ingredient.name] = ingredient.currentQuantity;
            CreateIngredientUI(ingredient.name, ingredient.currentQuantity);
        }

        Debug.Log("InventoryManager: Inventory initialized with " + ingredientStock.Count + " ingredients.");
    }

    public bool HasEnoughIngredients(List<Ingredients> requiredIngredients)
    {
        foreach (var ingredient in requiredIngredients)
        {
            string ingredientName = GetIngredientName(ingredient); 

            if (!ingredientStock.ContainsKey(ingredientName) || ingredientStock[ingredientName] < 1)
            {
                Debug.LogWarning($"InventoryManager: Not enough {ingredientName} (Required: 1, Available: {GetIngredientStock(ingredientName)})");
                return false;
            }
        }
        return true;
    }

    private string GetIngredientName(Ingredients ingredient)
    {
        Dictionary<Ingredients, string> ingredientNameMap = new Dictionary<Ingredients, string>
    {
        { Ingredients.TacoShells, "Taco Shells" },
        { Ingredients.Lettuce, "Lettuce" },
        { Ingredients.Sauce, "Sauce" },
        { Ingredients.Chicken, "Chicken" },
        { Ingredients.Onion, "Onion" },
        { Ingredients.Corn, "Corn" }
    };

        return ingredientNameMap.TryGetValue(ingredient, out string mappedName) ? mappedName : ingredient.ToString();
    }

    public void UseIngredients(List<Ingredients> usedIngredients)
    {
        foreach (var ingredient in usedIngredients)
        {
            string ingredientName = GetIngredientName(ingredient);

            if (ingredientStock.ContainsKey(ingredientName))
            {
                ingredientStock[ingredientName] -= 1; 
                if (ingredientStock[ingredientName] < 0)
                    ingredientStock[ingredientName] = 0;

                UpdateIngredientUI(ingredientName, ingredientStock[ingredientName]);
                Debug.Log($"InventoryManager: Used 1 of {ingredientName}. Remaining: {ingredientStock[ingredientName]}");
            }
        }
    }

    public int GetIngredientStock(string ingredientName)
    {
        return ingredientStock.TryGetValue(ingredientName, out int stock) ? stock : 0;
    }

    private void CreateIngredientUI(string ingredientName, int quantity)
    {
        GameObject uiElement = Instantiate(ingredientPrefab, inventoryContent);

        TextMeshProUGUI nameText = uiElement.GetComponentInChildren<TextMeshProUGUI>();
        Button restockButton = uiElement.GetComponentInChildren<Button>();

        nameText.text = $"{ingredientName} x{quantity}";

        if (restockButton != null)
        {
            restockButton.interactable = false;
        }

        ingredientUIElements[ingredientName] = uiElement;
    }

    private void UpdateIngredientUI(string ingredientName, int quantity)
    {
        if (!ingredientUIElements.ContainsKey(ingredientName))
        {
            Debug.LogWarning($"InventoryManager: UI element for {ingredientName} not found. Skipping update.");
            return;
        }

        GameObject uiElement = ingredientUIElements[ingredientName];

        TextMeshProUGUI nameText = uiElement.GetComponentInChildren<TextMeshProUGUI>();
        if (nameText == null)
        {
            Debug.LogError($"InventoryManager: 'Item Name' TextMeshProUGUI not found in {ingredientName} UI element.");
            return;
        }

        nameText.text = $"{ingredientName} x{quantity}";
    }

    private void ClearUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
        ingredientUIElements.Clear();
    }

    public void AddIngredients(string ingredientName, int amount)
    {
        if (ingredientStock.ContainsKey(ingredientName))
        {
            ingredientStock[ingredientName] += amount;
        }
        else
        {
            ingredientStock[ingredientName] = amount;
            CreateIngredientUI(ingredientName, amount);
        }

        UpdateIngredientUI(ingredientName, ingredientStock[ingredientName]);
        Debug.Log($"InventoryManager: Added {amount} of {ingredientName}. New Total: {ingredientStock[ingredientName]}");
    }
}
