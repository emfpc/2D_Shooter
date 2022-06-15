using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static Action<bool> OnGetIsPlayerDead;

    [Header("Player's Status")]
    [SerializeField] private int _lives = 3;
    private int _speed;
    private int _normalSpeed = 5;
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
    private int _speedPowerUp = 8;
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

    //Audio Variable Section
    [Header("Audio Clips")]
    [SerializeField] private AudioClip _fireLaserAudioClipEffect;
    [SerializeField] private AudioClip _powerUpsAudioClipEffect;
    private AudioSource _audioSorce;

    //Phase I
    private Animator _cameraAnimator;
    private bool _isHeatMisseleActive = false;
    private float _heatMisseleShotActiveSeconds = 10f;
    private WaitForSeconds _heatMisseleShotWaitForSeconds;
    [SerializeField] private bool _canThrusterBeUse=true;
    private int _thrustersSpeed = 10;
    [SerializeField] private float _fillMinus;
    [SerializeField] private float _fillPlus;
    private int _shieldLifeSpan = 3;
    private int _ammo = 15;
    private SpriteRenderer _shieldSpriteRenderer;
    [SerializeField] private GameObject _heatMisselePrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        _uiManager.PlayerLivesDisplay(_lives);
        _uiManager.UpdatePlayerAmmoCount(_ammo);
        _uiManager.ThrusterSlider(100);

        _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        _audioSorce = GetComponent<AudioSource>();

        _tripleShotWaitForSeconds = new WaitForSeconds(_tripleShotActiveSeconds);
        _speedWaitForSeconds = new WaitForSeconds(_speedActiveSeconds);
        _heatMisseleShotWaitForSeconds = new WaitForSeconds(_heatMisseleShotActiveSeconds);
        _shieldSpriteRenderer = _shieldGameObject.GetComponent<SpriteRenderer>();
        _cameraAnimator = Camera.main.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootingLaser();
        PlayerMovement();

        if(_canThrusterBeUse == true)
        {
            Thruster();
        }else if (_canThrusterBeUse == false){
            ReplenishThruster();
        }
        Debug.Log(_inputManager.ThrustersAction());
    }

    void ShootingLaser()
    {
        if (_inputManager.FireAction() && Time.time > _canIFire && _ammo > 0)
        {
            _ammo--;
            _uiManager.UpdatePlayerAmmoCount(_ammo);

            _canIFire = Time.time + _firingRate;

            _audioSorce.PlayOneShot(_fireLaserAudioClipEffect);

            if (_isTrippleShootActive == true && _isHeatMisseleActive == false)
            {
                InstantiateLasers(_tripleLaserPrefab);

            }
            else if (_isHeatMisseleActive == true && _isTrippleShootActive == false)
            {
                Instantiate(_heatMisselePrefab);
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

    void Thruster()
    {
        if (_inputManager.ThrustersAction() == true )
        {
            _fillMinus = 100 - (10.0f * Time.time);
            _uiManager.ThrusterSlider(_fillMinus);

            if (_fillMinus <= 0)
            {
                _fillMinus = 0;
                _canThrusterBeUse = false;
                return;
            }

            PlayerSpeed(_thrustersSpeed);
            return;
        }

        PlayerSpeed(_normalSpeed);
    }

    void ReplenishThruster()
    {
        _fillPlus = 0.0f + (3.0f * Time.time);
        _uiManager.ThrusterSlider(_fillPlus);

        if (_fillPlus >= 100)
        {
            _fillPlus = 0;
            _canThrusterBeUse = true;
            return;
        }
    }

    void  PlayerSpeed(int playerSpeed)
    {
        _speed = playerSpeed;
    }

    public void DamagePlayerLives()
    {
        _cameraAnimator.SetTrigger("CameraShake");
        if (_isShieldActive == true)
        {
            _shieldLifeSpan--;

            if(_shieldLifeSpan < 1)
                ActivateShields(false);

            switch (_shieldLifeSpan)
            {
                case 2:
                    _shieldSpriteRenderer.color = Color.yellow;
                    break;
                case 1:
                    _shieldSpriteRenderer.color = Color.red;
                    break;
                default:
                    break;
            }
            return;
        }
        
        _lives--;

        PlayerHurtStatus(_lives);

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

    public void StartHeatMisseleCoroutine()
    {
        StartCoroutine(ActivateHeatMissele());
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

    public void ActivatePowerUpSoudEffect()
    {
        _audioSorce.PlayOneShot(_powerUpsAudioClipEffect);
    }

    IEnumerator ActivateTripleShot()
    {
        _isTrippleShootActive = true;
        yield return _tripleShotWaitForSeconds;
        _isTrippleShootActive = false;
    }
    IEnumerator ActivateHeatMissele()
    {
        _isHeatMisseleActive = true;
        yield return _heatMisseleShotWaitForSeconds;
        _isHeatMisseleActive = false;
    }
    IEnumerator IncreaseSpeed()
    {
        PlayerSpeed(_speedPowerUp);
        yield return _speedWaitForSeconds;
        PlayerSpeed(_speed);
    }
    #endregion

    #region CollectableSection
    public void ReplenishAmmo()
    {
        _ammo = 15;
    }

    public void ReplenishLives()
    {
        if(_lives < 3)
        {
            _lives++;
            _uiManager.PlayerLivesDisplay(_lives);
            PlayerHurtStatus(_lives);
        }
    }
    #endregion
    public void AddPointsToScore()
    {
        _score += 10;
        _uiManager.UpdatePlayerScore(_score);
    }

    public void PlayerHurtStatus(int hurtStatus)
    {
        switch (hurtStatus)
        {
            case 2:
                _leftEngineGameObject.SetActive(true);
                _rightEngineGameObject.SetActive(false);
                break;
            case 1:
                _leftEngineGameObject.SetActive(true);
                _rightEngineGameObject.SetActive(true);
                break;
            default:
                _leftEngineGameObject.SetActive(false);
                _rightEngineGameObject.SetActive(false);
                break;
        }
    }
}