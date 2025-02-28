using System.Collections.Generic;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    [SerializeField] private Keybinds defaultKeybinds;

    private Dictionary<string, KeyCode> currentKeybinds;

    private readonly HashSet<KeyCode> restrictedKeys = new HashSet<KeyCode>
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.Escape, KeyCode.LeftWindows, KeyCode.RightWindows
    };

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

        LoadDefaultKeybinds();
    }

    private void LoadDefaultKeybinds()
    {
        currentKeybinds = new Dictionary<string, KeyCode>();

        foreach (var keybind in defaultKeybinds.defaultKeybinds)
        {
            currentKeybinds[keybind.actionName] = keybind.key;
        }
    }

    public KeyCode GetKey(string actionName)
    {
        if (currentKeybinds.TryGetValue(actionName, out KeyCode key))
        {
            return key;
        }
        return KeyCode.None;
    }

    public void SetKey(string actionName, KeyCode newKey)
    {
        if (restrictedKeys.Contains(newKey))
        {
            Debug.LogWarning($"Key {newKey} is restricted and cannot be assigned.");
            return;
        }

        if (currentKeybinds.ContainsKey(actionName))
        {
            KeyCode currentKey = currentKeybinds[actionName];

            if (currentKeybinds.ContainsValue(newKey))
            {
                string conflictAction = FindActionByKey(newKey);
                if (conflictAction != null)
                {
                    currentKeybinds[conflictAction] = currentKey;
                    Debug.Log($"Key {newKey} was already assigned to {conflictAction}. Swapped keys.");
                }
            }

            currentKeybinds[actionName] = newKey;
        }
        else
        {
            Debug.LogWarning($"Action name {actionName} not found in keybinds.");
        }
    }

    private string FindActionByKey(KeyCode key)
    {
        foreach (var keybind in currentKeybinds)
        {
            if (keybind.Value == key)
            {
                return keybind.Key;
            }
        }
        return null;
    }

    public void ResetToDefault()
    {
        LoadDefaultKeybinds();
    }
}