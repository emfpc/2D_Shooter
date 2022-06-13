using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInputs;
    private InputAction _moveAction;
    private InputAction _fireAction;
    private InputAction _restartAction;
    private InputAction _escapeAction;

    private void Start()
    {
        _playerInputs = GetComponent<PlayerInput>();
        _moveAction = _playerInputs.actions["Move"];
        _fireAction = _playerInputs.actions["Fire"];
        _restartAction = _playerInputs.actions["Restart"];
        _escapeAction = _playerInputs.actions["Escape"];
    }

    public Vector2 MoveAction()
    {
        return _moveAction.ReadValue<Vector2>();
    }

    public bool FireAction()
    {
        return _fireAction.WasPressedThisFrame();
    }

    public bool RestartAction()
    {
        return _restartAction.WasPressedThisFrame();
    }

    public bool EscapeAction()
    {
        return _escapeAction.WasPressedThisFrame();
    }
}
