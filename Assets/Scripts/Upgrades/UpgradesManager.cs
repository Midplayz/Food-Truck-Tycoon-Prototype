using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    [field: Header("Transition Manager")]
    [field: SerializeField] private Camera normalCamera;
    [field: SerializeField] private Camera upgradesCamera;
    [field: SerializeField] private Light mainLight;
    [field: SerializeField] private GameObject upgradesVisuals;
    [field: SerializeField] private GameObject normalUI;
    [field: SerializeField] private Button upgradesButton;
    [field: SerializeField] private Button backButton;
    private bool isInUpgrades = false;

    private void Start()
    {
        isInUpgrades = false;
        normalCamera.gameObject.SetActive(true);
        upgradesCamera.gameObject.SetActive(false);
        upgradesVisuals.SetActive(false);
        normalUI.SetActive(true);

        upgradesButton.onClick.AddListener(() =>
        {
            isInUpgrades = !isInUpgrades;
            SwitchCameras();
        });

        backButton.onClick.AddListener(() =>
        {
            isInUpgrades = !isInUpgrades;
            SwitchCameras();
        });
    }

    private void SwitchCameras()
    {
        normalCamera.gameObject.SetActive(!isInUpgrades);
        upgradesCamera.gameObject.SetActive(isInUpgrades);

        normalUI.SetActive(!isInUpgrades);

        mainLight.gameObject.SetActive(!isInUpgrades);
        upgradesVisuals.SetActive(isInUpgrades);
    }
}
