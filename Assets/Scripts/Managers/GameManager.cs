using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isPlayerDead = false;

    private InputManager _inputManager;

    private void Start()
    {
        _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    private void OnEnable()
    {
        Player.OnGetIsPlayerDead += IsPlayerDead;
    }
    private void Update()
    {
        if(_isPlayerDead == true & _inputManager.RestartAction())
        {
            SceneManager.LoadScene(1);
        }

        if (_inputManager.EscapeAction() == true)
            Application.Quit();
    }

    public void IsPlayerDead(bool playerLiveStatus)
    {
        _isPlayerDead = playerLiveStatus;
    }

    private void OnDisable()
    {
        Player.OnGetIsPlayerDead -= IsPlayerDead;
    }
}
