using UnityEngine;

public class Trigger2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneConversion.Instance.SwitchScenes(SceneType.Scene3D);
        }
    }
}
