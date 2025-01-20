using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IngredientStoreManager : MonoBehaviour
{
    public InventoryManager inventoryManager; 
    public GameManager gameManager; 

    [System.Serializable]
    public class IngredientButton
    {
        public string ingredientName; 
        public int pricePerUnit; 
        public int amountToBuy = 10;
        public TextMeshProUGUI buttonText;
    }

    public List<IngredientButton> ingredientButtons = new List<IngredientButton>();

    void Start()
    {
        UpdateAllButtonTexts();
    }

    public void BuyIngredient(string ingredientName)
    {
        IngredientButton button = ingredientButtons.Find(b => b.ingredientName == ingredientName);

        if (button != null)
        {
            int totalCost = button.pricePerUnit * button.amountToBuy;

            if (gameManager.totalIncome >= totalCost) 
            {
                gameManager.totalIncome -= totalCost;
                gameManager.UpdateIncome();
                inventoryManager.AddIngredients(ingredientName, button.amountToBuy); 
                Debug.Log($"Bought {button.amountToBuy} of {ingredientName} for ${totalCost}. Remaining Income: ${gameManager.totalIncome}");
                UpdateButtonText(button); 
            }
            else
            {
                Debug.LogWarning($"Not enough money to buy {button.amountToBuy} of {ingredientName}. Required: ${totalCost}, Available: ${gameManager.totalIncome}");
            }
        }
        else
        {
            Debug.LogError($"Ingredient '{ingredientName}' not found in ingredientButtons list!");
        }
    }

    void UpdateButtonText(IngredientButton button)
    {
        int totalCost = button.pricePerUnit * button.amountToBuy;
        button.buttonText.text = $"x{button.amountToBuy} for ${totalCost}";
    }

    public void UpdateAllButtonTexts()
    {
        foreach (var button in ingredientButtons)
        {
            UpdateButtonText(button);
        }
    }
}