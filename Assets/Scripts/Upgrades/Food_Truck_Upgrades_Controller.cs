using UnityEngine;

public class Food_Truck_Upgrades_Controller : MonoBehaviour
{
    [field: Header("Rotation Values")]
    [field: SerializeField] private float autoRotationSpeed = 10f;
    [field: SerializeField] float dragRotationSpeed = 5f; 
    private bool isDragging = false; 
    private float dragStartX; 
    private float rotationY;

    [field: Header("Rotation Values")]
    [field: SerializeField] private GameObject rustedTruck;
    [field: SerializeField] private GameObject cleanTruck;
    [field: SerializeField] private GameObject sushiTopper;
    [field: SerializeField] private GameObject roof;
    [field: SerializeField] private GameObject chef;

    private void Update()
    {
        if (isDragging)
        {
            float mouseDeltaX = Input.GetAxis("Mouse X");
            rotationY -= mouseDeltaX * dragRotationSpeed;
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
        else
        {
            transform.Rotate(0, autoRotationSpeed * Time.deltaTime, 0, Space.World);
            rotationY = transform.rotation.eulerAngles.y; 
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        dragStartX = Input.mousePosition.x;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }
}
