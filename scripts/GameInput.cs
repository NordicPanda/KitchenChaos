using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPause;

    public enum KeyBinding
    {
        Up,
        Down,
        Left,
        Right,
        Interact,
        AltInteract,
        Pause
    }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public string GetBindingText(KeyBinding keyBinding)
    {
        switch (keyBinding)
        {
            case KeyBinding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case KeyBinding.AltInteract:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case KeyBinding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case KeyBinding.Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case KeyBinding.Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case KeyBinding.Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case KeyBinding.Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            default:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
        }
    }

    public void Rebind(KeyBinding keyBinding)
    {
        OptionsUI.Instance.EnableBindingVisual();

        InputAction inputAction;
        int bindingIndex;

        switch (keyBinding)
        {
            default:
            case KeyBinding.Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case KeyBinding.Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case KeyBinding.Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case KeyBinding.Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case KeyBinding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case KeyBinding.AltInteract:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case KeyBinding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        playerInputActions.Player.Disable();

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(
            callback => {
                OptionsUI.Instance.DisableBindingVisual();
                callback.Dispose();
                playerInputActions.Player.Enable();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            } ).Start();
    }
    public Vector2 GetNormalizedMovementVector() => playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
}