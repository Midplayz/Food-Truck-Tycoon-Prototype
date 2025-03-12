using UnityEngine;
using System.Collections;

public class PickupSystem : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private bool shouldDrop = true;
    [SerializeField] private FoodPrep foodPrep;

    private GameObject heldItem = null;
    private Interactable currentInteractable = null;

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (heldItem != null)
            {
                if (shouldDrop)
                {
                    DropItem();
                }
                else
                {
                    CheckForSpecialInteraction();
                }
            }
            else if (currentInteractable != null && currentInteractable.IsPickable())
            {
                PickupItem(currentInteractable.gameObject);
                foodPrep.ClearCurrentItem();
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
        if (heldItem == null) return;

        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.transform.SetParent(null);
        heldItem = null;
    }

    private void CheckForSpecialInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.CompareTag("Dustbin"))
            {
                DisposeItem();
            }
            else if (hit.collider.CompareTag("Window"))
            {
                ServeItem();
            }
            else
            {
                Debug.Log("Cannot drop item here!");
            }
        }
        else
        {
            Debug.Log("No valid target detected!");
        }
    }

    private void DisposeItem()
    {
        Debug.Log("Item disposed!");
        Destroy(heldItem);
        heldItem = null;
    }

    private void ServeItem()
    {
        if (heldItem == null)
        {
            Debug.Log("No item to serve!");
            return;
        }

        SpawnedPrefab spawnedPrefab = heldItem.GetComponent<SpawnedPrefab>();

        if (spawnedPrefab == null || spawnedPrefab.menuItem == null)
        {
            Debug.LogError("Held item does not contain a valid SpawnedPrefab component with a MenuItem!");
            return;
        }

        MenuItem servedItem = spawnedPrefab.menuItem;
        Debug.Log($"Attempting to serve: {servedItem.name}");

        OrderingSystem.Instance.FulfillOrder(servedItem);

        Destroy(heldItem);
        heldItem = null;
        Debug.Log("Item served successfully!");
    }

}
