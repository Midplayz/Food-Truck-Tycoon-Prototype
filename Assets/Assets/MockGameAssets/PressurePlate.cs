using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    [Header("Pressure Plate Settings")]
    [SerializeField] private bool isActivated;

    [Header("Door Objects")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    [Header("Door Positions")]
    [SerializeField] private Vector3 leftDoorOpenPosition;
    [SerializeField] private Vector3 rightDoorOpenPosition;
    [SerializeField] private float doorMoveSpeed = 2f;

    private Vector3 leftDoorClosedPosition;
    private Vector3 rightDoorClosedPosition;

    private Coroutine leftDoorCoroutine;
    private Coroutine rightDoorCoroutine;

    private void Start()
    {
        isActivated = false;

        if (leftDoor == null || rightDoor == null)
        {
            UtilityScript.LogError("Left or right door not assigned!");
            return;
        }

        leftDoorClosedPosition = leftDoor.transform.localPosition;
        rightDoorClosedPosition = rightDoor.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeightedObject") || other.CompareTag("Player"))
        {
            isActivated = true;
            UtilityScript.Log("Pressure plate activated by " + other.tag);
            OnPressurePlateActivated();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WeightedObject") || other.CompareTag("Player"))
        {
            isActivated = false;
            UtilityScript.Log("Pressure plate deactivated by " + other.tag);
            OnPressurePlateDeactivated();
        }
    }

    private void OnPressurePlateActivated()
    {
        OpenAndCloseDoor();
    }

    private void OnPressurePlateDeactivated()
    {
        OpenAndCloseDoor();
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public void OpenAndCloseDoor()
    {
        if (isActivated)
        {
            if (leftDoorCoroutine != null) StopCoroutine(leftDoorCoroutine);
            if (rightDoorCoroutine != null) StopCoroutine(rightDoorCoroutine);
            leftDoorCoroutine = StartCoroutine(MoveDoor(leftDoor, leftDoorOpenPosition));
            rightDoorCoroutine = StartCoroutine(MoveDoor(rightDoor, rightDoorOpenPosition));
        }
        else
        {
            if (leftDoorCoroutine != null) StopCoroutine(leftDoorCoroutine);
            if (rightDoorCoroutine != null) StopCoroutine(rightDoorCoroutine);
            leftDoorCoroutine = StartCoroutine(MoveDoor(leftDoor, leftDoorClosedPosition));
            rightDoorCoroutine = StartCoroutine(MoveDoor(rightDoor, rightDoorClosedPosition));
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
