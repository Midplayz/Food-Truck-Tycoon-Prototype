using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController Instance { get; private set; }

    [Header("Cutscene References")]
    [SerializeField] private PlayableDirector[] cutscenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCutscene(int index)
    {
        if (index >= 0 && index < cutscenes.Length)
        {
            cutscenes[index].Play();
        }
        else
        {
            Debug.LogError("Cutscene index out of range!");
        }
    }

    public void StopCutscene(int index)
    {
        if (index >= 0 && index < cutscenes.Length)
        {
            cutscenes[index].Stop();
        }
        else
        {
            Debug.LogError("Cutscene index out of range!");
        }
    }
}
    