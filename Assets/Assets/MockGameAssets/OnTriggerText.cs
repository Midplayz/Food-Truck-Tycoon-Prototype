using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnTriggerText : MonoBehaviour
{
    [Header("Trigger Subs")]
    [SerializeField] private string textToShow;
    //private bool hasBeenTriggered = false;
    public void triggerText()
    {
        Debug.Log("Trigger Text Called");
        SubtitleManager.Instance.ClearSubtitle();
        SubtitleManager.Instance.ShowSubtitle(textToShow);
        //hasBeenTriggered = true;
    }
}
