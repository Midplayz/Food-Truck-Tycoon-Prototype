using UnityEngine;
public class ClickableCube : MonoBehaviour
{
    public int locationID;
    void OnMouseDown()
    {
        PreDayPrep.Instance.ClickOnLocation(locationID);
        Debug.Log("CLICKED");
    }
}
