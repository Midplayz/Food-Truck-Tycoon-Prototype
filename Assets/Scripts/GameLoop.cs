using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameLoop : MonoBehaviour
{
    public static GameLoop instance;

    [Header("UI Elements")]
    public TextMeshProUGUI clockText;
    public GameObject endOfDayPanel;
    public TextMeshProUGUI summaryText;
    public Button nextDayButton;

    [Header("Game Settings")]
    public int startHour = 10;
    public int endHour = 22;
    public float realSecondsPerGameHour = 60f;

    private DateTime gameTime;
    private float timeAccumulator = 0f;
    private bool isDayRunning = false;

    private int customersServed = 0;
    private int satisfiedCustomers = 0;
    private int dissatisfiedCustomers = 0;
    private int moneyEarned = 0;
    private int moneySpent = 0;

    private bool stopSpawning = false;

    public CustomerSpawner customerSpawner;

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
        endOfDayPanel.SetActive(false);
        nextDayButton.onClick.AddListener(PreDayPrep.Instance.StartPreDay);

        StartFirstDay();
    }

    private void StartFirstDay()
    {
        Cursor.lockState = CursorLockMode.None;
        StopCustomerSpawning();
        isDayRunning = false;
        endOfDayPanel.SetActive(false);
        PreDayPrep.Instance.StartPreDay();
    }

    private void Update()
    {
        if (isDayRunning)
        {
            UpdateClock();
        }
    }

    private void StartNewDay()
    {
        customersServed = 0;
        satisfiedCustomers = 0;
        dissatisfiedCustomers = 0;
        moneyEarned = 0;
        moneySpent = 0;

        gameTime = new DateTime(2022, 1, 1, startHour, 0, 0);
        isDayRunning = true;
        stopSpawning = false; 

        clockText.text = gameTime.ToString("hh:mm tt");
        endOfDayPanel.SetActive(false);
        customerSpawner.StartSpawningCustomers();
        Cursor.lockState = CursorLockMode.Locked;
        MovementValues.Instance.ToggleMovementCompletely(true);
    }

    private void UpdateClock()
    {
        timeAccumulator += Time.deltaTime;

        float realSecondsPerGameMinute = realSecondsPerGameHour / 60f;

        while (timeAccumulator >= realSecondsPerGameMinute)
        {
            gameTime = gameTime.AddMinutes(1);
            timeAccumulator -= realSecondsPerGameMinute;
        }

        clockText.text = gameTime.ToString("hh:mm tt");

        if (gameTime.Hour >= endHour && gameTime.Minute == 0)
        {
            StopCustomerSpawning(); 
        }
    }

    private void StopCustomerSpawning()
    {
        stopSpawning = true; 
        StartCoroutine(WaitForLastCustomer());
    }

    private System.Collections.IEnumerator WaitForLastCustomer()
    {
        while (FindAnyObjectByType<CustomerBehavior>() != null)
        {
            yield return null;
        }

        EndDay();
    }

    private void EndDay()
    {
        isDayRunning = false;
        customerSpawner.StopSpawningCustomers();
        MovementValues.Instance.ToggleMovementCompletely(false);
        Cursor.lockState = CursorLockMode.None;

        int profitOrLoss = moneyEarned - moneySpent;

        summaryText.text = $"Day Summary:\n" +
                           $"Customers Served: {customersServed}\n" +
                           $"Satisfied Customers: {satisfiedCustomers}\n" +
                           $"Dissatisfied Customers: {dissatisfiedCustomers}\n" +
                           $"Money Earned: ${moneyEarned}\n" +
                           $"Money Spent: ${moneySpent}\n" +
                           $"Profit/Loss: ${profitOrLoss}";

        endOfDayPanel.SetActive(true);
    }

    public void StartNextDay()
    {
        StartNewDay();
    }

    public void RegisterCustomer(bool satisfied, int income, int spent)
    {
        customersServed++;
        if (satisfied)
        {
            satisfiedCustomers++;
            moneyEarned += income;
        }
        else
        {
            dissatisfiedCustomers++;
        }

        moneySpent += spent;
    }

    public bool ShouldStopSpawning()
    {
        return stopSpawning;
    }
}
