using System;
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

    private DeckManager deckManager;

    private void Awake()
    {
        player_input_actions = new PlayerInputActions();
        deckManager = GetComponent<DeckManager>();
    }

    private void OnEnable()
    {
        movement = player_input_actions.Player.Movement;
        movement.Enable();
        aim = player_input_actions.Player.Aim;
        aim.Enable();

        player_input_actions.Player.Shoot.performed += HandleShooting;
        player_input_actions.Player.Shoot.Enable();

        player_input_actions.Player.Dash.performed += HandleDash;
        player_input_actions.Player.Dash.Enable();

        player_input_actions.Player.UseSingleCard.performed += HandleSingleCard;
        player_input_actions.Player.UseSingleCard.Enable();

        player_input_actions.Player.ShuffleDeck.performed += HandleDeckShuffle;
        player_input_actions.Player.ShuffleDeck.Enable();
        
        player_input_actions.Player.ReloadDeck.performed += HandleDeckReload;
        player_input_actions.Player.ReloadDeck.Enable();

        player_input_actions.Player.StackCards.performed += HandleSalvoDeck;
        player_input_actions.Player.StackCards.Enable();

        player_input_actions.Player.UseSalvo.performed += HandleSalvo;
        player_input_actions.Player.UseSalvo.Enable();
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

    private void HandleSingleCard(InputAction.CallbackContext obj)
    {
        deckManager.UseCard();
    }

    private void HandleDeckShuffle(InputAction.CallbackContext obj)
    {
        deckManager.ShuffleDeck();
    }
    
    private void HandleDeckReload(InputAction.CallbackContext obj)
    {
        deckManager.ReloadDeck();
    }

    private void HandleSalvoDeck(InputAction.CallbackContext obj)
    {
        deckManager.AddToSalvo();
    }

    private void HandleSalvo(InputAction.CallbackContext obj)
    {
        deckManager.UseSalvo();
    }

    private void HandleDash(InputAction.CallbackContext obj)
    {
        p_movement.Dash(movement.ReadValue<Vector2>());
    }
}
