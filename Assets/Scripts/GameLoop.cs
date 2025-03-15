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
    private float moneyEarned = 0;
    public float moneySpent = 0;

    private bool stopOrdering = false;

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
        StopOrdering();
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
        stopOrdering = false;

        clockText.text = gameTime.ToString("hh:mm tt");
        endOfDayPanel.SetActive(false);
        OrderingSystem.Instance.StartOrdering();
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
            StopOrdering();
        }
    }

    private void StopOrdering()
    {
        stopOrdering = true;
        OrderingSystem.Instance.StopOrdering();
        StartCoroutine(WaitForLastOrders());
    }

    private System.Collections.IEnumerator WaitForLastOrders()
    {
        while (OrderingSystem.Instance.GetCurrentOrderCount() > 0)
        {
            yield return null;
        }

        EndDay();
    }

    private void EndDay()
    {
        isDayRunning = false;
        MovementValues.Instance.ToggleMovementCompletely(false);
        Cursor.lockState = CursorLockMode.None;

        float profitOrLoss = moneyEarned - moneySpent;

        summaryText.text = $"Day Summary:\n" +
                           $"Orders Served: {customersServed}\n" +
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

    public void RegisterOrder(bool satisfied, float income, int spent)
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

    public bool ShouldStopOrdering()
    {
        return stopOrdering;
    }
}
