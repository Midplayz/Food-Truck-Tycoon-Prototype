using UnityEngine;

public class Trigger3D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneConversion.Instance.SwitchScenes(SceneType.Scene2D);
        }
    }
}
