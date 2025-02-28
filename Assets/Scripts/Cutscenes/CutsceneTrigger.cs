using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private int cutsceneIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneController.Instance.PlayCutscene(cutsceneIndex);
        }
    }
}