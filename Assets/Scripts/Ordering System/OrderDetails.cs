using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class OrderDetails : MonoBehaviour
{
    [Header("Order UI Components")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image orderBackground;

    [Header("Order Settings")]
    [SerializeField] private float minExpiryTime = 10f;
    [SerializeField] private float maxExpiryTime = 30f;

    private float expiryTime;
    private float timer;

    public int priceValue;
    public MenuItem assignedMenuItem;

    private void Start()
    {
        if (SavedValues.menuList == null || SavedValues.menuList.Count == 0)
        {
            Debug.LogError("No menu items available in SavedValues.menuList!");
            Destroy(gameObject);
            return;
        }

        AssignRandomMenuItem();
        StartCoroutine(OrderCountdown());
    }

    public void AssignRandomMenuItem()
    {
        assignedMenuItem = SavedValues.menuList[Random.Range(0, SavedValues.menuList.Count)];

        itemNameText.text = assignedMenuItem.name;
        priceValue= assignedMenuItem.price;

        expiryTime = Random.Range(minExpiryTime, maxExpiryTime);
        timer = expiryTime;
    }

    private IEnumerator OrderCountdown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            float progress = timer / expiryTime;
            orderBackground.color = Color.Lerp(new Color(1f, 0.3f, 0.3f, 1f), new Color(0.3f, 1f, 0.3f, 1f), progress);

            yield return null;
        }

        Debug.Log($"Order expired: {assignedMenuItem.name}");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (OrderingSystem.Instance != null)
        {
            OrderingSystem.Instance.RemoveOrder(this.gameObject);
        }
    }

}
