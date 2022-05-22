using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions player_input_actions;
    private InputAction movement;
    private InputAction aim;

    private PlayerMovement p_movement;
    private PlayerShooting p_shooting;

    private void Awake()
    {
        player_input_actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movement = player_input_actions.Player.Movement;
        movement.Enable();
        aim = player_input_actions.Player.Aim;
        aim.Enable();

        player_input_actions.Player.Shoot.started += HandleShooting;
        player_input_actions.Player.Shoot.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        aim.Disable();
        player_input_actions.Player.Shoot.Disable();
    }

    private void Start()
    {
        p_movement = GetComponent<PlayerMovement>();
        p_shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        p_shooting.AimCursor(aim.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        p_movement.Move(movement.ReadValue<Vector2>(), p_shooting.Is_Aiming);
    }

    private void HandleShooting(InputAction.CallbackContext obj)
    {
        p_shooting.Shoot();
    }
}
