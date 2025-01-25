using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    [field: Header("Transition Manager")]
    [field: SerializeField] private Camera normalCamera;
    [field: SerializeField] private Camera upgradesCamera;
    [field: SerializeField] private Light mainLight;
    [field: SerializeField] private GameObject upgradesVisuals;
    [field: SerializeField] private GameObject normalUI;
    [field: SerializeField] private GameObject upgradesUI;
    [field: SerializeField] private Button upgradesButton;
    [field: SerializeField] private Image fadeOverlay;
    [field: SerializeField] private float fadeDuration = 0.5f;
    [field: SerializeField] private foodTruckUpgradesController foodTruckUpgradesController;

    private bool isInUpgrades = false;

    private void Start()
    {
        isInUpgrades = false;
        normalCamera.gameObject.SetActive(true);
        upgradesCamera.gameObject.SetActive(false);
        upgradesVisuals.SetActive(false);
        normalUI.SetActive(true);
        upgradesUI.SetActive(false);

        fadeOverlay.color = new Color(0, 0, 0, 0);

        upgradesButton.onClick.AddListener(() =>
        {
            isInUpgrades = !isInUpgrades;
            StartCoroutine(SwitchWithFade());
        });
    }

    private System.Collections.IEnumerator SwitchWithFade()
    {
        yield return StartCoroutine(Fade(1));

        if(isInUpgrades)
        {
            foodTruckUpgradesController.UpdateUI();
        }

        normalCamera.gameObject.SetActive(!isInUpgrades);
        upgradesCamera.gameObject.SetActive(isInUpgrades);

        normalUI.SetActive(!isInUpgrades);
        upgradesUI.SetActive(isInUpgrades);
        mainLight.gameObject.SetActive(!isInUpgrades);
        upgradesVisuals.SetActive(isInUpgrades);

        upgradesButton.GetComponentInChildren<TextMeshProUGUI>().text = isInUpgrades ? "Back" : "Upgrades";

        yield return StartCoroutine(Fade(0));
    }

    private System.Collections.IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeOverlay.color.a;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }

        fadeOverlay.color = new Color(0, 0, 0, targetAlpha);
    }
}
