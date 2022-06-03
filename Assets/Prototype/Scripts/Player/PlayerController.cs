using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput player_input;
    private PlayerControls player_controls;
    private InputAction movement;
    private InputAction aim;

    private PlayerMovement p_movement;
    private PlayerShooting p_shooting;

    private DeckManager deckManager;
    private Inventory inventory;

    [SerializeField]
    private bool is_gamepad;

    private void Awake()
    {
        player_controls = new PlayerControls();
        player_input = GetComponent<PlayerInput>();
        deckManager = GetComponent<DeckManager>();
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        player_controls.Enable();
        movement = player_controls.Player.Movement;
        aim = player_controls.Player.Aim;

        player_controls.Player.Shoot.performed += HandleShooting;

        player_controls.Player.Reload.performed += HandleManualReload;

        player_controls.Player.Dash.performed += HandleDash;

        player_controls.Player.UseSingleCard.performed += HandleSingleCard;

        player_controls.Player.ShuffleDeck.performed += HandleDeckShuffle;
        
        player_controls.Player.ReloadDeck.performed += HandleDeckReload;

        player_controls.Player.StackCards.performed += HandleSalvoDeck;

        player_controls.Player.UseSalvo.performed += HandleSalvo;

        player_controls.Player.Interact.performed += HandleInteraction;
    }

    private void OnDisable()
    {
        player_controls.Disable();
    }

    private void Start()
    {
        p_movement = GetComponent<PlayerMovement>();
        p_shooting = GetComponent<PlayerShooting>();
    }

    private void Update()
    {
        //Debug.Log(aim.phase);
        //TODO: limit mouse aiming to range; only call when mouse has moved (gamepad should still work normally)
        p_shooting.AimCursor(aim.ReadValue<Vector2>(), is_gamepad);
    }

    private void FixedUpdate()
    {
        p_movement.Move(movement.ReadValue<Vector2>(), p_shooting.Is_Aiming);
    }

    public void OnDeviceChange(PlayerInput pI)
    {
        is_gamepad = pI.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    private void HandleShooting(InputAction.CallbackContext obj)
    {
        p_shooting.Shoot();
    }
    
    private void HandleManualReload(InputAction.CallbackContext obj)
    {
        p_shooting.ManualReload();
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

    private void HandleInteraction(InputAction.CallbackContext obj)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Interactable"))
            {
                Interactable interactable = colliders[i].GetComponent<Interactable>();
                foreach (Keys key in inventory.My_Keys)
                {
                    interactable.Interact(key);
                }
            }
        }
    }
}
