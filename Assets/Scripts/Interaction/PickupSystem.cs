using UnityEngine;
using System.Collections;

public class PickupSystem : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPosition; // Position where the item will be held
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Default interaction key

    private GameObject heldItem = null;
    private Interactable currentInteractable = null;

    private void Update()
    {
        // Check for interaction input
        if (Input.GetKeyDown(interactionKey))
        {
            if (heldItem != null)
            {
                DropItem();
            }
            else if (currentInteractable != null && currentInteractable.IsPickable())
            {
                PickupItem(currentInteractable.gameObject);
            }
        }
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
    }

    private void PickupItem(GameObject item)
    {
        heldItem = item;
        //item.GetComponent<Collider>().enabled = false;
        item.GetComponent<Rigidbody>().isKinematic = true;

        StartCoroutine(MoveItemToPosition(item.transform, holdPosition, 0.5f, item));
    }

    private IEnumerator MoveItemToPosition(Transform itemTransform, Transform targetPosition, float duration, GameObject item)
    {
        Vector3 startPosition = itemTransform.position;
        Quaternion startRotation = itemTransform.rotation;

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            itemTransform.position = Vector3.Lerp(startPosition, targetPosition.position, t);
            itemTransform.rotation = Quaternion.Slerp(startRotation, targetPosition.rotation, t);
            time += Time.deltaTime;
            yield return null;
        }

        itemTransform.SetParent(targetPosition);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        item.GetComponent<Collider>().enabled = false;
    }

    private void DropItem()
    {
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.transform.SetParent(null);
        heldItem = null;
    }
}
