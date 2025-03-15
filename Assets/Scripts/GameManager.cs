using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float totalIncome = 0.0f;
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

    public void RegisterOrderFulfillment(bool satisfied, float income)
    {
        if (satisfied)
        {
            satisfiedCustomers++;
            totalIncome += (income * PreDayPrep.Instance.incomeMultiplier);
            Debug.Log("Order fulfilled successfully! Income: " + totalIncome);
        }
        else
        {
            dissatisfiedCustomers++;
            Debug.Log("Order was not needed or incorrect.");
        }

        GameLoop.instance.RegisterOrder(satisfied, income, 0);
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