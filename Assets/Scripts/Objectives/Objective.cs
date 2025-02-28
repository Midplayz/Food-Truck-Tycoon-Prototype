using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;

    public abstract void StartObjective();
    public abstract void UpdateObjective();
    public abstract void CompleteObjective();
}