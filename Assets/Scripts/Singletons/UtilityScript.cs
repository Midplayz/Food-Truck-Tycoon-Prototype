using UnityEngine;

public static class UtilityScript
{
    public static void Log(string message)
    {
#if UNITY_EDITOR || DEBUG
        Debug.Log(message);
#endif
    }

    public static void LogWarning(string message)
    {
#if UNITY_EDITOR || DEBUG
        Debug.LogWarning(message);
#endif
    }

    public static void LogError(string message)
    {
#if UNITY_EDITOR || DEBUG
        Debug.LogError(message);
#endif
    }
}
