using UnityEngine;

public class UpgradesReflection : MonoBehaviour
{
    public static UpgradesReflection instance;

    [Header("Truck Upgrades")]
    public bool truckPurchased = false;
    public bool topperPurchased = false;
    public bool roofPurchased = false;
    public bool chefPurchased = false;

    [Header("Truck Parts")]
    [SerializeField] private GameObject rustedTruck;
    [SerializeField] private GameObject cleanTruck;
    [SerializeField] private GameObject sushiTopper;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject chef;

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

    public void UpdateVisuals()
    {
        if (!cleanTruck.activeInHierarchy && truckPurchased)
        {
            rustedTruck.SetActive(false);
            cleanTruck.SetActive(true);
        }
        if (!sushiTopper.activeInHierarchy && topperPurchased)
        {
            sushiTopper.SetActive(true);
        }
        if (!roof.activeInHierarchy && roofPurchased)
        {
            roof.SetActive(true);
        }
        if (!chef.activeInHierarchy && chefPurchased)
        {
            chef.SetActive(true);
        }
    }
}
