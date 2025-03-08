using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SavedValues
{
    public static int selectedLocation;
    public static int startTime;
    public static int endTime;
    public static List<MenuItem> menuList;
}

public enum Ingredients
{
    TacoShells,
    Nachos,
    Beef,
    Chicken,
    Pork,
    Beans,
    Guacamole,
    PicoDeGallo,
    Salsa,
    Cheese,
    SourCream
}

public class PreDayPrep : MonoBehaviour
{
    public static PreDayPrep Instance { get; private set; }

    [field: Header("General")]
    [SerializeField] private GameObject preDayPanel;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private GameObject locationSelection;
    [SerializeField] private GameObject workTimings;
    [SerializeField] private GameObject menuPlans;
    [SerializeField] private GameObject ingredientsPurchasing;

    [field: Header("Location Selection")]
    public List<LocationStats> locationStats = new List<LocationStats>();
    [SerializeField] private GameObject locationPrefab;
    [SerializeField] private GameObject locationView;

    [field: Header("Work Timings")]
    [SerializeField] private TMP_Dropdown startTimeDropdown;
    [SerializeField] private TMP_Dropdown endTimeDropdown;
    [SerializeField] private Button submitTimingsButton;

    [field: Header("Menu Plan")]
    [SerializeField] private ScrollRect menuScrollRect;
    [SerializeField] private GameObject menuItemPrefab;
    [SerializeField] private Button menuContinueButton;
    public List<MenuItem> menuList = new List<MenuItem>();
    private List<Toggle> menuToggles = new List<Toggle>();

    [field: Header("Ingredients Purchasing")]
    [SerializeField] private ScrollRect ingredientsScrollRect;
    [SerializeField] private GameObject ingredientsItemPrefab;
    [SerializeField] private Button ingredientsContinueButton;
    [SerializeField] private GameObject RestockPanel;
    [SerializeField] private TextMeshProUGUI restockTitle;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI cashInHand;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private Button confirmRestockButton;
    [SerializeField] private Button cancelRestockButton;
    public List<IngredientItem> ingredientsList = new List<IngredientItem>();

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

    void Start()
    {
        workTimings.SetActive(false);
        menuPlans.SetActive(false);
        ingredientsPurchasing.SetActive(false);

        startTimeDropdown.onValueChanged.AddListener(delegate { ValidateWorkTimings(); });
        endTimeDropdown.onValueChanged.AddListener(delegate { ValidateWorkTimings(); });
        submitTimingsButton.onClick.AddListener(SubmitWorkTimings);
    }

    public void StartPreDay()
    {
        title.text = "Select Location";
        preDayPanel.SetActive(true);
        PopulateLocation();
        locationSelection.SetActive(true);
    }

    private void PopulateLocation()
    {
        foreach (Transform child in locationView.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < locationStats.Count; i++)
        {
            LocationStats location = locationStats[i];

            GameObject locationItem = Instantiate(locationPrefab, locationView.transform);

            TextMeshProUGUI locationText = locationItem.GetComponentInChildren<TextMeshProUGUI>();
            location.selectButton = locationItem.GetComponentInChildren<Button>();

            locationText.text = $"{location.name}: {location.description}. " +
                                $"Income Multiplier: {location.incomeMultiplier}x. " +
                                $"Customers Multiplier: {location.customerSpawnRateMultipler}x. " +
                                $"Competition: {location.competitionMultiplier}x. " +
                                $"Crime: {location.crimeMultiplier}x.";

            int index = i;
            location.selectButton.onClick.AddListener(() => SelectLocation(index));
        }
    }

    private void SelectLocation(int location)
    {
        Debug.Log("Location " + location + " selected");
        SavedValues.selectedLocation = location;
        locationSelection.SetActive(false);
        title.text = "Select Work Timings";
        workTimings.SetActive(true);
    }

    private void ValidateWorkTimings()
    {
        int startTime = startTimeDropdown.value;
        int endTime = endTimeDropdown.value;

        if (endTime > startTime && (endTime - startTime) >= 1)
        {
            submitTimingsButton.interactable = true;
            submitTimingsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Submit";
        }
        else
        {
            submitTimingsButton.interactable = false;
            submitTimingsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Invalid Time Selection!";
        }
    }

    private void SubmitWorkTimings()
    {
        SavedValues.startTime = int.Parse(startTimeDropdown.options[startTimeDropdown.value].text.Split(':')[0]);
        SavedValues.endTime = int.Parse(endTimeDropdown.options[endTimeDropdown.value].text.Split(':')[0]);
        GameLoop.instance.startHour = SavedValues.startTime;
        GameLoop.instance.endHour = SavedValues.endTime;
        workTimings.SetActive(false);
        title.text = "Plan Menu";
        menuPlans.SetActive(true);
        PopulateMenuList();
    }

    private void PopulateMenuList()
    {
        foreach (Transform child in menuScrollRect.content)
        {
            Destroy(child.gameObject);
        }

        menuToggles.Clear();

        foreach (MenuItem item in menuList)
        {
            GameObject menuItemObject = Instantiate(menuItemPrefab, menuScrollRect.content);

            TextMeshProUGUI itemText = menuItemObject.GetComponentInChildren<TextMeshProUGUI>();
            Toggle itemToggle = menuItemObject.GetComponentInChildren<Toggle>();
            TMP_InputField priceInput = menuItemObject.GetComponentInChildren<TMP_InputField>();

            itemText.text = item.name;
            itemToggle.isOn = item.isActive;
            priceInput.text = item.price > 0 ? item.price.ToString() : "";

            menuToggles.Add(itemToggle);

            itemToggle.onValueChanged.AddListener(delegate { ValidateMenuSelection(); });
            priceInput.onValueChanged.AddListener(delegate { ValidateMenuSelection(); });
        }

        menuContinueButton.interactable = false;
        menuContinueButton.onClick.AddListener(ConfirmMenu);
    }

    private void ValidateMenuSelection()
    {
        for (int i = 0; i < menuToggles.Count; i++)
        {
            Toggle toggle = menuToggles[i];
            TMP_InputField priceInput = menuScrollRect.content.GetComponentsInChildren<TMP_InputField>()[i];

            if (toggle.isOn && (string.IsNullOrEmpty(priceInput.text) || int.Parse(priceInput.text) <= 0))
            {
                menuContinueButton.interactable = false;
                return;
            }
        }

        menuContinueButton.interactable = menuToggles.Exists(t => t.isOn);
    }

    private void ConfirmMenu()
    {
        Debug.Log("Menu Confirmed!");

        SavedValues.menuList = new List<MenuItem>();

        for (int i = 0; i < menuList.Count; i++)
        {
            menuList[i].isActive = menuToggles[i].isOn;
            TMP_InputField priceInput = menuScrollRect.content.GetComponentsInChildren<TMP_InputField>()[i];

            if (!string.IsNullOrEmpty(priceInput.text) && int.TryParse(priceInput.text, out int price) && price > 0)
            {
                menuList[i].price = price;

                if (menuList[i].isActive)
                {
                    SavedValues.menuList.Add(new MenuItem
                    {
                        name = menuList[i].name,
                        isActive = menuList[i].isActive,
                        price = menuList[i].price
                    });
                }
            }
        }

        menuPlans.SetActive(false);
        PopulateIngredients();
        title.text = "Restock Ingredients";
        ingredientsPurchasing.SetActive(true);
    }

    private void PopulateIngredients()
    {
        RestockPanel.SetActive(false);
        foreach (Transform child in ingredientsScrollRect.content)
        {
            Destroy(child.gameObject);
        }

        foreach (IngredientItem ingredient in ingredientsList)
        {
            GameObject ingredientObject = Instantiate(ingredientsItemPrefab, ingredientsScrollRect.content);

            TextMeshProUGUI nameText = ingredientObject.GetComponentInChildren<TextMeshProUGUI>();
            Button restockButton = ingredientObject.GetComponentInChildren<Button>();

            nameText.text = $"{ingredient.name} x{ingredient.currentQuantity}";

            restockButton.onClick.AddListener(() => RestockButtonPressed(ingredient.ingredientReferance));
        }

        ingredientsContinueButton.interactable = true;
        ingredientsContinueButton.onClick.AddListener(ConfirmIngredientsSelection);
    }

    private void RestockButtonPressed(Ingredients ingredient)
    {
        IngredientItem selectedIngredient = ingredientsList.Find(item => item.ingredientReferance == ingredient);
        if (selectedIngredient == null) return;


        restockTitle.text = $"Restock {selectedIngredient.name}";
        itemInfo.text = $"MOQ: {selectedIngredient.moq} | ${selectedIngredient.price} Per Piece";
        cashInHand.text = "Cash Balance: {Cash}";

        quantityInput.text = "";
        confirmRestockButton.interactable = false;

        RestockPanel.SetActive(true);
        
        quantityInput.onValueChanged.RemoveAllListeners();
        quantityInput.onValueChanged.AddListener(delegate {
            int quantity;
            if (int.TryParse(quantityInput.text, out quantity) && quantity >= selectedIngredient.moq)
            {
                confirmRestockButton.interactable = true;
            }
            else
            {
                confirmRestockButton.interactable = false;
            }
        });

        confirmRestockButton.onClick.RemoveAllListeners();
        confirmRestockButton.onClick.AddListener(() => ConfirmRestockPressed(ingredient, int.Parse(quantityInput.text)));

        cancelRestockButton.onClick.RemoveAllListeners();
        cancelRestockButton.onClick.AddListener(CancelRestockPressed);
    }

    private void ConfirmRestockPressed(Ingredients ingredient, int quantity)
    {
        IngredientItem selectedIngredient = ingredientsList.Find(item => item.ingredientReferance == ingredient);
        if (selectedIngredient == null) return;
        selectedIngredient.currentQuantity += quantity;
        PopulateIngredients();
        RestockPanel.SetActive(false);
    }

    private void CancelRestockPressed()
    {
        RestockPanel.SetActive(false);
    }

    private void ConfirmIngredientsSelection()
    {
        InventoryManager.Instance.InitializeInventory(ingredientsList);
        Debug.Log("Ingredients Confirmed!");
        ingredientsPurchasing.SetActive(false);
        preDayPanel.SetActive(false);
        GameLoop.instance.StartNextDay();
    }
}

[System.Serializable]
public class MenuItem
{
    public string name;
    public bool isActive;
    public List<Ingredients> ingredients;
    public int price;
}

[System.Serializable]
public class LocationStats
{
    public string name;
    public string description;
    public float incomeMultiplier = 1.0f;
    public float customerSpawnRateMultipler = 1.0f;
    public float competitionMultiplier = 1.0f;
    public float crimeMultiplier = 1.0f;
    public Button selectButton;
}

[System.Serializable]
public class IngredientItem
{
    public string name;
    public int moq;
    public int price;
    public int currentQuantity;
    public bool isPerishable;
    public Ingredients ingredientReferance;
}
