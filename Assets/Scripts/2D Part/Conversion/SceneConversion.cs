using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SceneType
{
    Scene2D,
    Scene3D
}

public class SceneConversion : MonoBehaviour
{
    public static SceneConversion Instance { get; private set; }

    [SerializeField] private Camera camera2D;
    [SerializeField] private Camera camera3D;
    [SerializeField] private PlayerMovement2D playerMovement2D;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private SceneType currentScene = SceneType.Scene3D;

    [field: SerializeField] private Image fadeOverlay;
    [field: SerializeField] private float fadeDuration = 0.5f;

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

    private void Start()
    {
        fadeOverlay.color = new Color(0, 0, 0, 0);

        camera2D.gameObject.SetActive(false);
        camera3D.gameObject.SetActive(true);
        playerMovement2D.canMove = false;
        MovementValues.Instance.ToggleMovementCompletely(true);
    }

    public void SwitchScenes(SceneType newScene)
    {
        if (newScene == currentScene) return; 

        currentScene = newScene;

        if (newScene == SceneType.Scene2D)
        {
            StartCoroutine(SwitchWithFade(true, false));

            //playerMovement2D.OnSwitched();

            MovementValues.Instance.ToggleMovementCompletely(false);
        }
        else
        {
            StartCoroutine(SwitchWithFade(false, true));

            //firstPersonController.ReturnToInitialPosition();
            playerMovement2D.canMove = false;

            //MovementValues.Instance.ToggleMovementCompletely(true);
        }
    }

    private System.Collections.IEnumerator SwitchWithFade(bool cam2D, bool cam3D)
    {
        yield return StartCoroutine(Fade(1));

        camera2D.gameObject.SetActive(cam2D);
        camera3D.gameObject.SetActive(cam3D);

        if(currentScene == SceneType.Scene2D)
        {
            playerMovement2D.OnSwitched();
        }
        else
        {
            firstPersonController.ReturnToInitialPosition();
            MovementValues.Instance.ToggleMovementCompletely(true);
        }

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
