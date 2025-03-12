using UnityEngine;

public class InteractionMechanic : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private Camera playerCamera;

    private Interactable currentInteractable;
    private PickupSystem pickupSystem;

    private float lastCheckTime = 0f;
    private float checkInterval = 0.1f;

    private void Start()
    {
        pickupSystem = GetComponent<PickupSystem>();
        if (pickupSystem == null)
        {
            Debug.LogError("PickupSystem not found on the player!");
        }
    }

    void Update()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.OnInteract();
        }
    }

    private void FixedUpdate()
    {
        if (Time.time >= lastCheckTime + checkInterval)
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer)
    && hit.collider.TryGetComponent(out Interactable interactable))
        {
            if (currentInteractable != interactable)
            {
                /* if (currentInteractable != null) currentInteractable.OnLoseFocus(); */

                currentInteractable = interactable;
                /* currentInteractable.OnFocus(); */
                pickupSystem.SetCurrentInteractable(currentInteractable);
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                /* currentInteractable.OnLoseFocus(); */
                currentInteractable = null;
                pickupSystem.SetCurrentInteractable(null);
            }
        }
    }
}
