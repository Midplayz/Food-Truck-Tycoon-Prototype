using UnityEngine;

[CreateAssetMenu(fileName = "MovementDefaultValues", menuName = "Settings/MovementDefaultValues")]
public class MovementDefaultValues : ScriptableObject
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 2f;
    public float crouchHeight = 1f;
    public float gravity = -9.81f;
    public float crouchSpeedMultiplier = 0.5f;
    public float mouseSensitivity = 100f;

    public bool canJump = true;
    public bool canWalk = true;
    public bool canSprint = true;
    public bool canCrouch = true;
}
