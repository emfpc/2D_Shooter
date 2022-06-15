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
    private InputAction _thrustersAction;

    private void Start()
    {
        _playerInputs = GetComponent<PlayerInput>();
        _moveAction = _playerInputs.actions["Move"];
        _fireAction = _playerInputs.actions["Fire"];
        _restartAction = _playerInputs.actions["Restart"];
        _escapeAction = _playerInputs.actions["Escape"];
        _thrustersAction = _playerInputs.actions["Thrusters"];
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

    public bool ThrustersAction()
    {
        bool thrustersAction = _thrustersAction.ReadValue<float>() != 0 ? true : false;
        return thrustersAction;
    }
}
