using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Action<bool> OnGetIsPlayerDead;

    [Header("Player's Status")]
    [SerializeField] private int _speed;
    [SerializeField] private int _lives = 3;
    private Vector3 _movement;
    private float _horizontalInput;
    private float _verticalInput;

    //Projectiles Variable Section
    [Header("Lasers Prefabs and Container")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _lasersContainer;

    //Hurt Engine Section
    [Header("Players Animations GameObject Fields")]
    [SerializeField] private GameObject _leftEngineGameObject;
    [SerializeField] private GameObject _rightEngineGameObject;

    //Cooldown Variable Section
    private float _firingRate = 0.2f;
    private float _canIFire = -1f;

    //Managers Variable Section
    private UIManager _uiManager;
    private InputManager _inputManager;

    //PowerUp Variable Section
    private bool _isTrippleShootActive = false;
    private float _tripleShotActiveSeconds = 10f;
    private WaitForSeconds _tripleShotWaitForSeconds;

    private float _speedActiveSeconds = 8f;
    private WaitForSeconds _speedWaitForSeconds;

    [Tooltip("This object in a Child of the Player object and it will only be enable when Player picks the Power Up Shield")]
    [SerializeField] GameObject _shieldGameObject;
    private bool _isShieldActive = false;
    
    //Score Variable Section
    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        _uiManager.PlayerLivesDisplay(_lives);

        _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        _tripleShotWaitForSeconds = new WaitForSeconds(_tripleShotActiveSeconds);
        _speedWaitForSeconds = new WaitForSeconds(_speedActiveSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        ShootingLaser();
        PlayerMovement();
    }

    void ShootingLaser()
    {
        if (_inputManager.FireAction() && Time.time > _canIFire)
        {
            _canIFire = Time.time + _firingRate;

            if(_isTrippleShootActive == true)
            {
                InstantiateLasers(_tripleLaserPrefab);
            }
            else
            {
                InstantiateLasers(_laserPrefab);
            }
        }
    }

    void InstantiateLasers(GameObject laserObject)
    {
        Instantiate(laserObject, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, _lasersContainer.transform);
    }

    void PlayerMovement()
    {
        _horizontalInput = _inputManager.MoveAction().x;
        _verticalInput = _inputManager.MoveAction().y;

        _movement = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(_movement * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void DamagePlayerLives()
    {
        if (_isShieldActive == true)
        {
            ActivateShields(false);
            return;
        }
        
        _lives--;

        switch (_lives)
        {
            case 2:
                _leftEngineGameObject.SetActive(true);
            break;
            case 1:
                _rightEngineGameObject.SetActive(true);
                break;
        }

        _uiManager.PlayerLivesDisplay(_lives);

        if(_lives == 0)
        {
            OnGetIsPlayerDead(true);
            Destroy(this.gameObject);
        }
    }
    #region PowerUpsSection
    public void StartTripleShotCoroutine()
    {
        StartCoroutine(ActivateTripleShot());
    }

    public void StartSpeedCoroutine()
    {
        StartCoroutine(IncreaseSpeed());
    }

    public void ActivateShields(bool activateShield)
    {
        _isShieldActive = activateShield;
        _shieldGameObject.SetActive(activateShield);
    }

    IEnumerator ActivateTripleShot()
    {
        _isTrippleShootActive = true;
        yield return _tripleShotWaitForSeconds;
        _isTrippleShootActive = false;
    }

    IEnumerator IncreaseSpeed()
    {
        var oldSpeed = _speed;
        _speed = 8;
        yield return _speedWaitForSeconds;
        _speed = oldSpeed;
    }
    #endregion

    public void AddPointsToScore()
    {
        _score += 10;
        _uiManager.UpdatePlayerScore(_score);
    }
}