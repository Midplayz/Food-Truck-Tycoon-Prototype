using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Camera mainCamera; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) 
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Customer")) 
            {
                Debug.Log($"Clicked on {hit.collider.name}");
                CustomerBehavior customer = hit.collider.GetComponent<CustomerBehavior>();

                if (customer != null)
                {
                    customer.FulfillOrder(inventoryManager);
                    Debug.Log("Order fulfilled for customer.");
                }
                else
                {
                    Debug.LogError("CustomerBehavior script not found on the clicked Customer object!");
                }
            }
        }
    }
}
