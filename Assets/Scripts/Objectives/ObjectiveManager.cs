using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static ObjectiveManager instance;
    public static ObjectiveManager Instance => instance;

    private List<Objective> objectives = new List<Objective>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddObjective(Objective objective)
    {
        objectives.Add(objective);
        ObjectiveUI.Instance.AddObjectiveUI(objective);
    }

    public void UpdateObjective(Objective objective)
    {
        ObjectiveUI.Instance.UpdateObjectiveUI(objective);
    }

    public void CompleteObjective(Objective objective)
    {
        objectives.Remove(objective);
        ObjectiveUI.Instance.RemoveObjectiveUI(objective);
    }
}