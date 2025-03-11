using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalIncome = 0;
    public int satisfiedCustomers = 0;
    public int dissatisfiedCustomers = 0;
    public InventoryManager inventoryManager;
    public Camera mainCamera;

    public Slider satisfactionSlider;
    public Image sliderFill;
    public TextMeshProUGUI incomeAmount;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        satisfactionSlider.value = 1.0f;
        sliderFill.color = Color.green;
        UpdateIncome();
    }

    public void RegisterOrderFulfillment(bool satisfied, int income)
    {
        if (satisfied)
        {
            satisfiedCustomers++;
            totalIncome += income;
            Debug.Log("Order fulfilled successfully! Income: " + totalIncome);
        }
        else
        {
            dissatisfiedCustomers++;
            Debug.Log("Order was not needed or incorrect.");
        }

        UpdateSatisfactionSlider();
        UpdateIncome();
    }

    void UpdateSatisfactionSlider()
    {
        int totalCustomers = satisfiedCustomers + dissatisfiedCustomers;

        if (totalCustomers == 0)
        {
            satisfactionSlider.value = 1.0f;
            sliderFill.color = Color.green;
            return;
        }

        float satisfactionPercentage = (float)satisfiedCustomers / totalCustomers;

        satisfactionSlider.value = satisfactionPercentage;
        sliderFill.color = Color.Lerp(Color.red, Color.green, satisfactionPercentage);
    }

    public void UpdateIncome()
    {
        incomeAmount.text = "$" + totalIncome.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            totalIncome = 6969;
            UpdateIncome();
        }
    }
}