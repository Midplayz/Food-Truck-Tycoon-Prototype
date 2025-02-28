using UnityEngine;

[CreateAssetMenu(fileName = "ProgressObjective", menuName = "Objectives/ProgressObjective")]
public class ProgressObjective : Objective
{
    public int requiredAmount;
    private int currentAmount;

    public override void StartObjective()
    {
        currentAmount = 0;
        ObjectiveManager.Instance.AddObjective(this);
    }

    public override void UpdateObjective()
    {
        // Update UI with current amount
    }

    public void IncrementProgress()
    {
        currentAmount++;
        ObjectiveManager.Instance.UpdateObjective(this);
        Debug.Log("Increased");
        if (currentAmount >= requiredAmount)
        {
            CompleteObjective();
            Debug.Log("Completed!");
        }
    }

    public override void CompleteObjective()
    {
        ObjectiveManager.Instance.CompleteObjective(this);
    }
}