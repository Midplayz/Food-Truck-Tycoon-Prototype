using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Food_Truck_Upgrades_Controller : MonoBehaviour
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

    [SerializeField] private int playerMoney = 100;

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
                if (!truckPurchased && playerMoney >= truckUpgradePrice)
                {
                    playerMoney -= truckUpgradePrice;
                    truckPurchased = true;
                    rustedTruck.SetActive(false);
                    cleanTruck.SetActive(true);
                }
                break;

            case "Topper":
                if (!topperPurchased && playerMoney >= topperPrice)
                {
                    playerMoney -= topperPrice;
                    topperPurchased = true;
                    sushiTopper.SetActive(true);
                }
                break;

            case "Roof":
                if (!roofPurchased && playerMoney >= roofPrice)
                {
                    playerMoney -= roofPrice;
                    roofPurchased = true;
                    roof.SetActive(true);
                }
                break;

            case "Chef":
                if (!chefPurchased && playerMoney >= chefPrice)
                {
                    playerMoney -= chefPrice;
                    chefPurchased = true;
                    chef.SetActive(true);
                }
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        truckButtonText.text = truckPurchased ? "Purchased" : $"Buy for ${truckUpgradePrice}";
        truckButton.interactable = !truckPurchased && playerMoney >= truckUpgradePrice;

        topperButtonText.text = topperPurchased ? "Purchased" : $"Buy for ${topperPrice}";
        topperButton.interactable = !topperPurchased && playerMoney >= topperPrice;

        roofButtonText.text = roofPurchased ? "Purchased" : $"Buy for ${roofPrice}";
        roofButton.interactable = !roofPurchased && playerMoney >= roofPrice;

        chefButtonText.text = chefPurchased ? "Purchased" : $"Buy for ${chefPrice}";
        chefButton.interactable = !chefPurchased && playerMoney >= chefPrice;
    }
}
