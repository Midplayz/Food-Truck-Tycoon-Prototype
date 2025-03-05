using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public enum LocationSelection
{
    UniversalStudios,
    ElNido
}

public class SavedValues
{
    public static LocationSelection selectedLocation;
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
    [SerializeField] private Button locationButton1;
    [SerializeField] private Button locationButton2;

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
        preDayPanel.SetActive(false);
        locationSelection.SetActive(false);
        workTimings.SetActive(false);
        menuPlans.SetActive(false);
        ingredientsPurchasing.SetActive(false);

        locationButton1.onClick.AddListener(() => SelectLocation(LocationSelection.UniversalStudios));
        locationButton2.onClick.AddListener(() => SelectLocation(LocationSelection.ElNido));

        startTimeDropdown.onValueChanged.AddListener(delegate { ValidateWorkTimings(); });
        endTimeDropdown.onValueChanged.AddListener(delegate { ValidateWorkTimings(); });
        submitTimingsButton.onClick.AddListener(SubmitWorkTimings);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPreDay();
        }
    }

    public void StartPreDay()
    {
        title.text = "Select Location";
        preDayPanel.SetActive(true);
        locationSelection.SetActive(true);
    }

    private void SelectLocation(LocationSelection location)
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

            if (item.price > 0)
            {
                priceInput.text = item.price.ToString();
            }
            else
            {
                priceInput.text = "";
            }

            menuToggles.Add(itemToggle);
        }

        menuContinueButton.interactable = false;
        foreach (Toggle toggle in menuToggles)
        {
            toggle.onValueChanged.AddListener(delegate { ValidateMenuSelection(); });
        }
    }

    private void ValidateMenuSelection()
    {
        foreach (Toggle toggle in menuToggles)
        {
            if (toggle.isOn)
            {
                menuContinueButton.interactable = true;
                return;
            }
        }
        menuContinueButton.interactable = false;
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
