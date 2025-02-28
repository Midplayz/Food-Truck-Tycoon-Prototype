using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveTitleText;
    [SerializeField] private TextMeshProUGUI objectiveDescriptionText;

    public void Setup(Objective objective)
    {
        objectiveTitleText.text = objective.objectiveName;
        objectiveDescriptionText.text = objective.objectiveDescription;
    }

    public void UpdateUI(Objective objective)
    {
        // Update UI based on objective progress
    }
}