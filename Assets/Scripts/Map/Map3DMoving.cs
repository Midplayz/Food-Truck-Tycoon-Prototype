using UnityEngine;

public class Map3DMoving : MonoBehaviour
{
    [Header("Pan Settings")]
    public float panSpeed = 1.0f;

    [Header("Bounds")]
    public Vector3 minBounds;
    public Vector3 maxBounds;

    [Header("Cameras")]
    public GameObject upgradeCamera;
    public GameObject camera3D;
    public GameObject camera2D;
    public GameObject mapCamera;

    [Header("Obj")]
    public GameObject canvasObj;

    private Vector3 lastMousePosition;
    private bool isDragging = false;

    void Update()
    {
        HandlePanning();
        HandleCameraSwitching();
    }

    private void HandlePanning()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            Vector3 move = new Vector3(-mouseDelta.x * panSpeed * 0.01f, 0, -mouseDelta.y * panSpeed * 0.01f);

            Vector3 newPosition = mapCamera.transform.position + move;

            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minBounds.z, maxBounds.z);

            mapCamera.transform.position = newPosition;
        }
    }

    private void HandleCameraSwitching()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mapCamera.activeSelf)
            {
                SetActiveCamera(camera3D);
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                canvasObj.SetActive(true);
            }
            else
            {
                SetActiveCamera(mapCamera);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                canvasObj.SetActive(false);
            }
        }
    }

    private void SetActiveCamera(GameObject activeCamera)
    {
        upgradeCamera.SetActive(false);
        camera3D.SetActive(false);
        camera2D.SetActive(false);
        mapCamera.SetActive(false);

        activeCamera.SetActive(true);
    }
}
