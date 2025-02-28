using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TimerObjective", menuName = "Objectives/TimerObjective")]
public class TimerObjective : Objective
{
    public float timerDuration;
    private float currentTime;

    public override void StartObjective()
    {
        currentTime = timerDuration;
        ObjectiveManager.Instance.AddObjective(this);
        CoroutineRunner.Instance.StartRoutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            ObjectiveManager.Instance.UpdateObjective(this);
            yield return null;
        }
        CompleteObjective();
    }

    public override void UpdateObjective()
    {
        // Update UI with remaining time
    }

    public override void CompleteObjective()
    {
        ObjectiveManager.Instance.CompleteObjective(this);
    }
}