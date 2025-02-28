using System.Collections;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    [Header("Subtitle Settings")]
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float displayDurationAfterTyping = 2f; 

    private Coroutine currentTypingCoroutine;
    private Coroutine clearCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        subtitleText.gameObject.SetActive(false); 
    }

    private void Start()
    {
        if (subtitleText == null)
        {
            UtilityScript.LogError("SubtitleText is not assigned!");
        }
    }

    public void ShowSubtitle(string message, Color? color = null)
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
        }

        if (clearCoroutine != null)
        {
            StopCoroutine(clearCoroutine);
        }

        subtitleText.color = color ?? Color.white;
        subtitleText.gameObject.SetActive(true); 
        currentTypingCoroutine = StartCoroutine(TypeSubtitle(message));
    }

    private IEnumerator TypeSubtitle(string message)
    {
        subtitleText.text = "";
        foreach (char c in message)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        clearCoroutine = StartCoroutine(ClearAfterDelay(displayDurationAfterTyping));
    }

    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearSubtitle();
    }

    public void ClearSubtitle()
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
            currentTypingCoroutine = null;
        }

        if (clearCoroutine != null)
        {
            StopCoroutine(clearCoroutine);
            clearCoroutine = null;
        }

        subtitleText.text = "";
        subtitleText.gameObject.SetActive(false); 
    }
}
