using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LocationData : MonoBehaviour
{
    public string locationName;
    public float customerTraffic;
    public float crimeRate;
    public float competitionRate;
    public float ingredientsPrice;
    public bool isLocked;

    public Button selectButton;

    private void Start()
    {
        if (selectButton != null)
        {
            selectButton.interactable = !isLocked;
        }

        selectButton.onClick.AddListener(() => { MockMapHandler.Instance.OpenLocationUI(locationName, customerTraffic, competitionRate, crimeRate, ingredientsPrice); });
    }
}
