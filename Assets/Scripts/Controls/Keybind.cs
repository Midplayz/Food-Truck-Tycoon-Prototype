using UnityEngine;

[System.Serializable]
public struct Keybind
{
    public string actionName;
    public KeyCode key;

    public Keybind(string actionName, KeyCode key)
    {
        this.actionName = actionName;
        this.key = key;
    }
}
