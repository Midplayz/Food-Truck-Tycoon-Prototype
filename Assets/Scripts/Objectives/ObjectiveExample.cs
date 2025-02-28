using UnityEngine;

public class ObjectiveExample : MonoBehaviour
{
    [SerializeField] private CheckboxObjective checkboxObjective;
    [SerializeField] private TimerObjective timerObjective;
    [SerializeField] private ProgressObjective progressObjective;

    private void Start()
    {
        if (checkboxObjective != null)
        {
            checkboxObjective.StartObjective();
        }
        if (timerObjective != null)
        {
            timerObjective.StartObjective();
        }
        if (progressObjective != null)
        {
            progressObjective.StartObjective();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            progressObjective.IncrementProgress();
        }
    }
}