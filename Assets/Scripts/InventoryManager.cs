using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, int> inventory = new Dictionary<string, int>()
    {
        { "Rice", 10 },
        { "Fish", 10 },
        { "Soy", 10 },
        { "Sesame", 10 },
        { "Cucumber", 10 }
    };

    public TextMeshProUGUI riceText;
    public TextMeshProUGUI fishText;
    public TextMeshProUGUI soyText;
    public TextMeshProUGUI sesameText;
    public TextMeshProUGUI cucumberText;

    void Start()
    {
        UpdateInventoryUI();
    }

    public bool UseIngredients(Dictionary<string, int> order)
    {
        foreach (var item in order)
        {
            if (!inventory.ContainsKey(item.Key) || inventory[item.Key] < item.Value)
            {
                Debug.Log("Not enough " + item.Key + " to fulfill order.");
                return false;
            }
        }

        foreach (var item in order)
        {
            inventory[item.Key] -= item.Value;
        }

        UpdateInventoryUI();

        return true;
    }

    public void AddIngredients(string ingredientName, int amount)
    {
        if (inventory.ContainsKey(ingredientName))
        {
            inventory[ingredientName] += amount;
            Debug.Log($"Added {amount} of {ingredientName}. New Total: {inventory[ingredientName]}");
            UpdateInventoryUI();
        }
        else
        {
            Debug.LogError($"Ingredient '{ingredientName}' does not exist in the inventory.");
        }
    }

    void UpdateInventoryUI()
    {
        riceText.text = inventory["Rice"].ToString();
        fishText.text = inventory["Fish"].ToString();
        soyText.text = inventory["Soy"].ToString();
        sesameText.text = inventory["Sesame"].ToString();
        cucumberText.text = inventory["Cucumber"].ToString();
    }
}
