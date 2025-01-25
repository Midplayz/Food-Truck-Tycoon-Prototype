using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalIncome = 0;
    public int satisfiedCustomers = 0;
    public int dissatisfiedCustomers = 0;

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

    void OnEnable()
    {
        CustomerBehavior.OnCustomerLeft += HandleCustomerLeft;
    }

    void OnDisable()
    {
        CustomerBehavior.OnCustomerLeft -= HandleCustomerLeft;
    }

    private void Start()
    {
        UpdateIncome();
    }

    void HandleCustomerLeft(bool satisfied, int income)
    {
        if (satisfied)
        {
            satisfiedCustomers++;
            totalIncome += income;
            Debug.Log("Customer satisfied! Income: " + totalIncome);
        }
        else
        {
            dissatisfiedCustomers++;
            Debug.Log("Customer left unhappy.");
        }

        UpdateSatisfactionSlider();
        UpdateIncome();
    }

    void UpdateSatisfactionSlider()
    {
        int totalCustomers = satisfiedCustomers + dissatisfiedCustomers;
        if (totalCustomers == 0) return; 

        float satisfactionPercentage = (float)satisfiedCustomers / totalCustomers;

        satisfactionSlider.value = satisfactionPercentage;

        sliderFill.color = Color.Lerp(Color.red, Color.green, satisfactionPercentage);
    }

    public void UpdateIncome()
    {
        incomeAmount.text = "$" + totalIncome.ToString();
    }
}
