using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] private Material openMaterial;
    [SerializeField] private Material closedMaterial;

    [Header("Door Objects")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("Door Positions")]
    [SerializeField] private Vector3 leftDoorOpenPosition;
    [SerializeField] private Vector3 rightDoorOpenPosition;
    [SerializeField] private float doorMoveSpeed = 2f;
    [SerializeField] private bool isDoorOpened = false;
    [SerializeField] private bool shouldDisableButton = false;

    private Vector3 leftDoorClosedPosition;
    private Vector3 rightDoorClosedPosition;
    private Renderer objRenderer;

    private Coroutine leftDoorCoroutine;
    private Coroutine rightDoorCoroutine;

    void Start()
    {
        if (leftDoor == null || rightDoor == null)
        {
            UtilityScript.LogError("Left or right door not assigned!");
            return;
        }

        leftDoorClosedPosition = leftDoor.transform.localPosition;
        rightDoorClosedPosition = rightDoor.transform.localPosition;

        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            UtilityScript.LogError("Renderer not found on the game object!");
        }
    }

    public void OpenAndCloseDoor()
    {
        if (objRenderer != null)
        {
            objRenderer.material = isDoorOpened ? closedMaterial : openMaterial;
        }

        if (shouldDisableButton)
        {
            gameObject.layer = 0;
        }

        if (!isDoorOpened)
        {
            if (leftDoorCoroutine != null) StopCoroutine(leftDoorCoroutine);
            if (rightDoorCoroutine != null) StopCoroutine(rightDoorCoroutine);
            leftDoorCoroutine = StartCoroutine(MoveDoor(leftDoor, leftDoorOpenPosition));
            rightDoorCoroutine = StartCoroutine(MoveDoor(rightDoor, rightDoorOpenPosition));
            isDoorOpened = !isDoorOpened;
            SubtitleManager.Instance.ShowSubtitle("Door Opening!");
        }
        else
        {
            if (leftDoorCoroutine != null) StopCoroutine(leftDoorCoroutine);
            if (rightDoorCoroutine != null) StopCoroutine(rightDoorCoroutine);
            leftDoorCoroutine = StartCoroutine(MoveDoor(leftDoor, leftDoorClosedPosition));
            rightDoorCoroutine = StartCoroutine(MoveDoor(rightDoor, rightDoorClosedPosition));
            isDoorOpened = !isDoorOpened;
            SubtitleManager.Instance.ShowSubtitle("Door Closing!");
        }
    }

    private IEnumerator MoveDoor(GameObject door, Vector3 targetPosition)
    {
        while (Vector3.Distance(door.transform.localPosition, targetPosition) > 0.01f)
        {
            door.transform.localPosition = Vector3.Lerp(door.transform.localPosition, targetPosition, doorMoveSpeed * Time.deltaTime);
            yield return null;
        }
        door.transform.localPosition = targetPosition;
    }
}
