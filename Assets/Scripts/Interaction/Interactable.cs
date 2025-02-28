using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 5f;
    private Outline outline;

    [Header("Interaction Event")]
    public UnityEvent onInteract;

    [Header("Pickup Settings")]
    [SerializeField] private bool isPickable = false; 

    private void Start()
    {
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = outlineColor;
        outline.OutlineWidth = outlineWidth;

        outline.enabled = false;
    }

    public void OnFocus()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void OnLoseFocus()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void OnInteract()
    {
        onInteract.Invoke();
    }

    public bool IsPickable()
    {
        return isPickable;
    }
}
