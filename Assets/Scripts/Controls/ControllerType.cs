using UnityEngine;

public enum ControllerType
{
    None,
    Xbox,
    PS,
    KeyboardMouse
}

public class ControllerDetector : MonoBehaviour
{
    public static ControllerType DetectController()
    {
        if (Input.GetJoystickNames().Length == 0)
        {
            return ControllerType.KeyboardMouse;
        }

        foreach (string joystickName in Input.GetJoystickNames())
        {
            if (joystickName.ToLower().Contains("xbox"))
            {
                return ControllerType.Xbox;
            }
            else if (joystickName.ToLower().Contains("wireless controller")) // PS4 controller name
            {
                return ControllerType.PS;
            }
        }

        return ControllerType.None;
    }
}