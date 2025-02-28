using System;
using UnityEngine;

public class MovementValues : MonoBehaviour
{
    public static MovementValues Instance { get; private set; }

    [Header("Walking and Running")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 100f;

    [Header("Jumping")]
    public float jumpHeight = 2f;
    public float gravity = -35f;

    [Header("Crouching and Crouch Walk")]
    public float crouchHeight = 1f;
    public float crouchSpeedMultiplier = 0.5f;

    [Header("Enable or Disable Abilities")]
    public bool canJump = true;
    public bool canWalk = true;
    public bool canSprint = true;
    public bool canCrouch = true;
    public bool canLookWithMouse = true;

    [Header("Scriptable Object For Default Values")]
    [SerializeField] private MovementDefaultValues defaultValuesData;

    void Awake()
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
    }

    public void ChangeWalkSpeed(float speed)
    {
        walkSpeed = speed;
        UtilityScript.Log("Walk Speed Changed to: " + speed);
    }

    public void ChangeSprintSpeed(float speed)
    {
        sprintSpeed = speed;
        UtilityScript.Log("Sprint Speed Changed to: " + speed);
    }

    public void ChangeJumpHeight(float height)
    {
        jumpHeight = height;
        UtilityScript.Log("Jump Height Changed to: " + height);
    }

    public void ChangeGravityValue(float newGravityValue)
    {
        gravity = newGravityValue;
        UtilityScript.Log("Gravity Value Changed to: " + newGravityValue);
    }

    public void ChangeCrouchHeight(float height)
    {
        crouchHeight = height;
        UtilityScript.Log("Crouch Height Changed to: " + height);
    }

    public void ChangeCrouchSpeedMultiplier(float speedMultiplier)
    {
        crouchSpeedMultiplier = speedMultiplier;
        UtilityScript.Log("Crouch Speed Multiplier Changed to: " + speedMultiplier);
    }

    public void ToggleCanJump(bool newCanJump)
    {
        canJump = newCanJump;
        UtilityScript.Log("Can Jump: " + newCanJump);
    }

    public void ToggleCanWalk(bool newCanWalk)
    {
        canWalk = newCanWalk;
        UtilityScript.Log("Can Walk: " + newCanWalk);
    }

    public void ToggleCanSprint(bool newCanSprint)
    {
        canSprint = newCanSprint;
        UtilityScript.Log("Can Sprint: " + newCanSprint);
    }

    public void ToggleCanCrouch(bool newCanCrouch)
    {
        canCrouch = newCanCrouch;
        UtilityScript.Log("Can Crouch: " + newCanCrouch);
    }

    public void ToggleCanMoveWithMouse(bool newCanLookWithMouse)
    {
        canLookWithMouse = newCanLookWithMouse;
        UtilityScript.Log("Can Look With Mouse: " + newCanLookWithMouse);
    }

    public void ToggleMovementCompletely(bool canMove)
    {
        ToggleCanCrouch(canMove);
        ToggleCanSprint(canMove);
        ToggleCanWalk(canMove);
        ToggleCanJump(canMove);
        ToggleCanMoveWithMouse(canMove);
    }

    public void ResetDefaults()
    {
        walkSpeed = defaultValuesData.walkSpeed;
        sprintSpeed = defaultValuesData.sprintSpeed;
        jumpHeight = defaultValuesData.jumpHeight;
        crouchHeight = defaultValuesData.crouchHeight;
        gravity = defaultValuesData.gravity;
        crouchSpeedMultiplier = defaultValuesData.crouchSpeedMultiplier;
        mouseSensitivity = defaultValuesData.mouseSensitivity;

        canJump = defaultValuesData.canJump;
        canWalk = defaultValuesData.canWalk;
        canSprint = defaultValuesData.canSprint;
        canCrouch = defaultValuesData.canCrouch;
        UtilityScript.Log("All Movement Values Reset to Default!");
    }
}
