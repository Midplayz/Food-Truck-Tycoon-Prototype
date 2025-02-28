using UnityEngine;

[CreateAssetMenu(fileName = "CheckboxObjective", menuName = "Objectives/CheckboxObjective")]
public class CheckboxObjective : Objective
{
    public bool isCompleted;

    public override void StartObjective()
    {
        isCompleted = false;
        ObjectiveManager.Instance.AddObjective(this);
    }

    public override void UpdateObjective()
    {
        // Update logic if needed
    }

    public override void CompleteObjective()
    {
        isCompleted = true;
        ObjectiveManager.Instance.CompleteObjective(this);
    }
}
