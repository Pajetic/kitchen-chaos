using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    private const string PLAYER_INPUT_BINDINGS = "PlayerInputBindings";

    public static GameInput Instance { get; private set; }
    public enum Binding {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlt,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlt,
        Gamepad_Pause
    }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnRebind;

    private PlayerInputActions playerInputActions;

    public string GetBindingText(Binding binding) { 
        switch (binding) {
            default:
            case Binding.MoveUp:
                return playerInputActions.Player.Movement.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputActions.Player.Movement.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputActions.Player.Movement.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputActions.Player.Movement.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlt:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void Rebind(Binding binding, Action onRebindCompleteCallback) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(binding) {
            default:
            case Binding.MoveUp:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = playerInputActions.Player.Movement;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlt:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(rebindingOperation => {
                rebindingOperation.Dispose();
                playerInputActions.Player.Enable();
                onRebindCompleteCallback?.Invoke();
                PlayerPrefs.SetString(PLAYER_INPUT_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                OnRebind?.Invoke(this, EventArgs.Empty);
            }).Start();
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Attempting to create multiple instances of GameInput.");
            Destroy(this);
        } else {
            Instance = this;
        }

        playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_INPUT_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_INPUT_BINDINGS));
        }
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += OnInteractPerformed;
        playerInputActions.Player.InteractAlternate.performed += OnInteractAlternatePerformed;
        playerInputActions.Player.Pause.performed += OnPause;
    }

    private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMovementVectorNormalized() {
        Vector2 moveInput = playerInputActions.Player.Movement.ReadValue<Vector2>();

        moveInput = moveInput.normalized;

        Vector3 moveVector = new Vector3(moveInput.x, 0f, moveInput.y);

        return moveVector;
    }

    private void OnInteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnInteractAlternatePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= OnInteractPerformed;
        playerInputActions.Player.InteractAlternate.performed -= OnInteractAlternatePerformed;
        playerInputActions.Player.Pause.performed -= OnPause;

        playerInputActions.Dispose();
    }
}


