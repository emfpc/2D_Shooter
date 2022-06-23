using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static Action<float> OnGetEnemyBossHasArrived;

    private bool _isPlayerDead = false;
    private bool _hasEnemyBossArrived = false;
    [SerializeField] private Camera _mainCamera;

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
        //CheckForEnemyBoss();
        Invoke("CheckForEnemyBoss", 3f);

        if(_isPlayerDead == true & _inputManager.RestartAction())
        {
            SceneManager.LoadScene(1);
        }

        if (_inputManager.EscapeAction() == true)
            Application.Quit();
    }

    void CheckForEnemyBoss()
    {
        var enemyBoss = GameObject.FindGameObjectsWithTag("EnemyBoss");
        foreach (var bossEnemy in enemyBoss)
            if (bossEnemy.activeInHierarchy == true)
            {
                _hasEnemyBossArrived = true;
                OnGetEnemyBossHasArrived(-7f);
            }
                
        /*
        if (_hasEnemyBossArrived == false)
            if (enemyBoss[0].activeInHierarchy == true)
                _hasEnemyBossArrived = true;
        */
        if (_hasEnemyBossArrived == true)
            _mainCamera.orthographicSize = 8;
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
