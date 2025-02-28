using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultKeybinds", menuName = "Settings/Keybinds")]
public class Keybinds : ScriptableObject
{
    public List<Keybind> defaultKeybinds;

    private Dictionary<string, Keybind> keybindsDictionary;

    public void OnEnable()
    {
        keybindsDictionary = new Dictionary<string, Keybind>();
        foreach (var keybind in defaultKeybinds)
        {
            keybindsDictionary[keybind.actionName] = keybind;
        }
    }

    public KeyCode GetDefaultKey(string actionName)
    {
        if (keybindsDictionary.TryGetValue(actionName, out Keybind keybind))
        {
            return keybind.key;
        }
        return KeyCode.None;
    }
}