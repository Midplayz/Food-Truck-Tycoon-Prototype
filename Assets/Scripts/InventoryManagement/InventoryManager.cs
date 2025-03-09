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
        Debug.Log("SetFaLKE");
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

    public bool HasEnoughIngredients(Dictionary<string, int> requiredIngredients)
    {
        foreach (var item in requiredIngredients)
        {
            if (!ingredientStock.ContainsKey(item.Key) || ingredientStock[item.Key] < item.Value)
            {
                Debug.LogWarning($"InventoryManager: Not enough {item.Key} (Required: {item.Value}, Available: {GetIngredientStock(item.Key)})");
                return false;
            }
        }
        return true;
    }

    public void UseIngredients(Dictionary<string, int> usedIngredients)
    {
        foreach (var item in usedIngredients)
        {
            if (ingredientStock.ContainsKey(item.Key))
            {
                ingredientStock[item.Key] -= item.Value;
                if (ingredientStock[item.Key] < 0)
                    ingredientStock[item.Key] = 0;

                UpdateIngredientUI(item.Key, ingredientStock[item.Key]);
                Debug.Log($"InventoryManager: Used {item.Value} of {item.Key}. Remaining: {ingredientStock[item.Key]}");
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
        if (ingredientUIElements.ContainsKey(ingredientName))
        {
            ingredientUIElements[ingredientName].transform.Find("IngredientQuantity").GetComponent<Text>().text = quantity.ToString();
        }
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
