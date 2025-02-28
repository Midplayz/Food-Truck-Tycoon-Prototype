using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance;

    [SerializeField] private RectTransform objectiveListParent;
    [SerializeField] private GameObject objectiveUIPrefab;

    private Dictionary<Objective, GameObject> objectiveUIs = new Dictionary<Objective, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddObjectiveUI(Objective objective)
    {
        GameObject objUI = Instantiate(objectiveUIPrefab, objectiveListParent);
        objUI.GetComponent<ObjectiveUIItem>().Setup(objective);
        objectiveUIs[objective] = objUI;
    }

    public void UpdateObjectiveUI(Objective objective)
    {
        if (objectiveUIs.TryGetValue(objective, out GameObject objUI))
        {
            objUI.GetComponent<ObjectiveUIItem>().UpdateUI(objective);
        }
    }

    public void RemoveObjectiveUI(Objective objective)
    {
        if (objectiveUIs.TryGetValue(objective, out GameObject objUI))
        {
            Destroy(objUI);
            objectiveUIs.Remove(objective);
        }
    }
}
