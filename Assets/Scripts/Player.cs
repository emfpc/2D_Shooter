using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _speed;
    private Vector3 _movement;
    private float _horizontalInput;
    private float _verticalInput;

    //Projectiles Variable Section
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _lasersContainer;

    //Cooldown Variable Section
    private float _firingRate = 0.2f;
    private float _canIFire = -1f;

    //Lives Variable Section
    [SerializeField] private int _lives = 3;

    //Managers Variable Section
    private SpawnManager[] _spawnManager;

    //PowerUp Variable Section
    private bool _isTrippleShootActive = false;
    private float _tripleShotActiveSeconds = 10f;
    private WaitForSeconds _tripleShotWaitForSeconds;

    private float _speedActiveSeconds = 8f;
    private WaitForSeconds _speedWaitForSeconds;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;

        _spawnManager = GameObject.Find("SpawnManager").GetComponents<SpawnManager>();
        if (_spawnManager == null)
            Debug.Log("SpanwManager is NULL");

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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canIFire)
        {
            _canIFire = Time.time + _firingRate;

            if(_isTrippleShootActive == true)
            {
                Instantiate(_tripleLaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, _lasersContainer.transform);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity, _lasersContainer.transform);
            }
        }
    }

    void PlayerMovement()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

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
        _lives--;
        if(_lives == 0)
        {
            foreach (var spawner in _spawnManager)
                spawner.IsPlayerDead(true);

            Destroy(this.gameObject);
        }
    }

    public void StartTripleShotCoroutine()
    {
        StartCoroutine(ActivateTripleShot());
    }

    public void StartSpeedCoroutine()
    {
        StartCoroutine(IncreaseSpeed());
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
}