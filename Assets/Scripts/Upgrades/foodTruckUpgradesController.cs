using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class foodTruckUpgradesController : MonoBehaviour
{
    [Header("Rotation Values")]
    [SerializeField] private float autoRotationSpeed = 10f;
    [SerializeField] private float dragRotationSpeed = 5f;
    private bool isDragging = false;
    private float rotationY;

    [Header("Truck Upgrades")]
    [SerializeField] private GameObject rustedTruck;
    [SerializeField] private GameObject cleanTruck;
    [SerializeField] private GameObject sushiTopper;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject chef;

    [Header("Upgrade Prices and Buttons")]
    [SerializeField] private int truckUpgradePrice = 50;
    [SerializeField] private int topperPrice = 30;
    [SerializeField] private int roofPrice = 40;
    [SerializeField] private int chefPrice = 70;

    [SerializeField] private Button truckButton;
    [SerializeField] private Button topperButton;
    [SerializeField] private Button roofButton;
    [SerializeField] private Button chefButton;

    [SerializeField] private TextMeshProUGUI truckButtonText;
    [SerializeField] private TextMeshProUGUI topperButtonText;
    [SerializeField] private TextMeshProUGUI roofButtonText;
    [SerializeField] private TextMeshProUGUI chefButtonText;

    private bool truckPurchased = false;
    private bool topperPurchased = false;
    private bool roofPurchased = false;
    private bool chefPurchased = false;

    private void Start()
    {
        UpdateUI();

        truckButton.onClick.AddListener(() => PurchaseUpgrade("Truck"));
        topperButton.onClick.AddListener(() => PurchaseUpgrade("Topper"));
        roofButton.onClick.AddListener(() => PurchaseUpgrade("Roof"));
        chefButton.onClick.AddListener(() => PurchaseUpgrade("Chef"));
    }

    private void Update()
    {
        if (isDragging)
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            rotationY -= mouseDeltaX * dragRotationSpeed;
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
        else
        {
            transform.Rotate(0, autoRotationSpeed * Time.deltaTime, 0, Space.World);
            rotationY = transform.rotation.eulerAngles.y;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void PurchaseUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Truck":
                if (!truckPurchased && GameManager.instance.totalIncome >= truckUpgradePrice)
                {
                    GameManager.instance.totalIncome -= truckUpgradePrice;
                    truckPurchased = true;
                    rustedTruck.SetActive(false);
                    cleanTruck.SetActive(true);
                }
                break;

            case "Topper":
                if (!topperPurchased && GameManager.instance.totalIncome >= topperPrice)
                {
                    GameManager.instance.totalIncome -= topperPrice;
                    topperPurchased = true;
                    sushiTopper.SetActive(true);
                }
                break;

            case "Roof":
                if (!roofPurchased && GameManager.instance.totalIncome >= roofPrice)
                {
                    GameManager.instance.totalIncome -= roofPrice;
                    roofPurchased = true;
                    roof.SetActive(true);
                }
                break;

            case "Chef":
                if (!chefPurchased && GameManager.instance.totalIncome >= chefPrice)
                {
                    GameManager.instance.totalIncome -= chefPrice;
                    chefPurchased = true;
                    chef.SetActive(true);
                }
                break;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        truckButtonText.text = $"${truckUpgradePrice}";
        truckButton.GetComponentInChildren<TextMeshProUGUI>().text = truckPurchased ? "Purchased" : "Buy";
        truckButton.interactable = !truckPurchased && GameManager.instance.totalIncome >= truckUpgradePrice;

        topperButtonText.text = $"${topperPrice}";
        topperButton.GetComponentInChildren<TextMeshProUGUI>().text = topperPurchased ? "Purchased" : "Buy";
        topperButton.interactable = !topperPurchased && GameManager.instance.totalIncome >= topperPrice;

        roofButtonText.text = $"${roofPrice}";
        roofButton.GetComponentInChildren<TextMeshProUGUI>().text = roofPurchased ? "Purchased" : "Buy";
        roofButton.interactable = !roofPurchased && GameManager.instance.totalIncome >= roofPrice;

        chefButtonText.text = $"${chefPrice}";
        chefButton.GetComponentInChildren<TextMeshProUGUI>().text = chefPurchased ? "Purchased" : "Buy";
        chefButton.interactable = !chefPurchased && GameManager.instance.totalIncome >= chefPrice;
    }
}
