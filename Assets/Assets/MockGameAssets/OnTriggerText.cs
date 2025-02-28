using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnTriggerText : MonoBehaviour
{
    [Header("Trigger Subs")]
    [SerializeField] private string textToShow;
    private bool hasBeenTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            SubtitleManager.Instance.ClearSubtitle();
            SubtitleManager.Instance.ShowSubtitle(textToShow);
            hasBeenTriggered = true;
        }
    }
}
