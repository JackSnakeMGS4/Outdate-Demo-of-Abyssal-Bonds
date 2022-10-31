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
    private InputAction mouse_aim;

    private PlayerMovement p_movement;
    private PlayerShooting p_shooting;
    private PlayerHealth p_health;

    private DeckManager deck_manager;
    private Inventory inventory;

    [SerializeField]
    private bool is_gamepad;
    [SerializeField]
    private PauseManager pause_manager;

    private void Awake()
    {
        player_controls = new PlayerControls();
        player_input = GetComponent<PlayerInput>();
        deck_manager = GetComponent<DeckManager>();
        inventory = GetComponent<Inventory>();
    }

    private void OnEnable()
    {
        player_controls.Player.Enable();
        AssignPlayerControls();
        AssignMenuControls();
    }

    private void AssignMenuControls()
    {
        //player_controls.Menu.ExitMenu.performed += _ => HandlePause();
    }

    private void AssignPlayerControls()
    {
        movement = player_controls.Player.Movement;
        aim = player_controls.Player.Aim;
        mouse_aim = player_controls.Player.MouseAim;

        player_controls.Player.Shoot.performed += HandleShooting;

        player_controls.Player.Reload.performed += HandleManualReload;

        player_controls.Player.Dash.performed += HandleDash;

        player_controls.Player.UseSingleCard.performed += HandleSingleCard;

        player_controls.Player.ShuffleDeck.performed += HandleDeckShuffle;

        player_controls.Player.ReloadDeck.performed += HandleDeckReload;

        player_controls.Player.StackCards.performed += HandleSalvoDeck;

        player_controls.Player.UseSalvo.performed += HandleSalvo;

        player_controls.Player.Interact.performed += HandleInteraction;

        player_controls.Player.PauseGame.performed += _ => HandlePause();

        player_controls.Player.RechargeShield.performed += HandleShieldRegen;
        player_controls.Player.DiscardCard.performed += HandleDiscard;
    }

    public void HandlePause()
    {
        if (PauseManager.is_paused)
        {
            player_controls.Menu.Disable();
            player_controls.Player.Enable();
        }
        else
        {
            player_controls.Player.Disable();
            player_controls.Menu.Enable();
        }

        pause_manager.DeterminePause();
    }

    private void OnDisable()
    {
        player_controls.Player.Disable();
    }

    private void Start()
    {
        p_movement = GetComponent<PlayerMovement>();
        p_shooting = GetComponent<PlayerShooting>();
        p_health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        HandleMouseAim();
    }

    private void HandleMouseAim()
    {
        if (is_gamepad)
        {
            p_shooting.AimCursor(aim.ReadValue<Vector2>());
        }
        else
        {
            //Debug.Log(mouse_aim.phase);
            if (mouse_aim.phase == InputActionPhase.Started || mouse_aim.phase == InputActionPhase.Performed)
            {
                p_shooting.AimCursorWithMouse(mouse_aim.ReadValue<Vector2>());
                p_shooting.Is_Aiming = true;
            }
            if (mouse_aim.phase == InputActionPhase.Waiting || mouse_aim.phase == InputActionPhase.Canceled)
            {
                p_shooting.Is_Aiming = false;
            }
        }
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
        if(p_shooting.Is_Aiming)
            deck_manager.UseCard();
    }

    private void HandleDiscard(InputAction.CallbackContext obj)
    {
        deck_manager.DiscardCard();
    }

    private void HandleDeckShuffle(InputAction.CallbackContext obj)
    {
        deck_manager.ShuffleDeck();
    }
    
    private void HandleDeckReload(InputAction.CallbackContext obj)
    {
        deck_manager.ReloadDeck();
    }

    private void HandleSalvoDeck(InputAction.CallbackContext obj)
    {
        deck_manager.AddToSalvo();
    }

    private void HandleSalvo(InputAction.CallbackContext obj)
    {
        if(p_shooting.Is_Aiming)
            deck_manager.UseSalvo();
    }

    private void HandleDash(InputAction.CallbackContext obj)
    {
        p_movement.Dash(movement.ReadValue<Vector2>());
    }

    private void HandleShieldRegen(InputAction.CallbackContext obj)
    {
        p_health.RechargeShield();
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
