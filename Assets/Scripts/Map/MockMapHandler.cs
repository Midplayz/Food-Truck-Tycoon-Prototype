using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MockMapHandler : MonoBehaviour
{
    public static MockMapHandler Instance;

    public GameObject locationInfoViewer;
    public TextMeshProUGUI locationName;
    public TextMeshProUGUI customerTraffic;
    public TextMeshProUGUI competitionRate;
    public TextMeshProUGUI crimeRate;
    public TextMeshProUGUI ingredientsPrice;
    public Button confirmButton;
    public Button cancelButton;

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
        locationInfoViewer.SetActive(false);
    }

    public void OpenLocationUI(string locationNameInput, float customerTrafficInput, float competitionRateInput, float crimeRateInput, float ingredientsPriceInput)
    {
        locationName.text = locationNameInput;
        customerTraffic.text = customerTrafficInput.ToString();
        competitionRate.text = competitionRateInput.ToString();
        crimeRate.text = crimeRateInput.ToString();
        ingredientsPrice.text = ingredientsPriceInput.ToString();

        cancelButton.onClick.AddListener (() => { locationInfoViewer.SetActive(false); });
        confirmButton.onClick.AddListener (() => { Debug.Log("Selected Location: " + locationNameInput); locationInfoViewer.SetActive(false); });

        locationInfoViewer.SetActive(true);
    }
}
