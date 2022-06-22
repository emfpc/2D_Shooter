using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : MonoBehaviour
{
    [SerializeField] private float _speed = 4.8f;
    private Player _player;

    private Animator _enemyAnimator;
    private int _enemyDestroyedTriggerID;

    private BoxCollider2D _boxCollider2D;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explotionEffectAudioClip;

    //Enemy Laser Object Variable Section
    [Header("Enemy Laser object field")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    private GameObject _enemyContainer;

    private bool _isEnemyDead = false;

    private SpawnSystem _spawnSystem;

    //Shooting Variable Section
    private float _firingRate = 3.0f;
    private float _canIFire = -1f;

    RaycastHit2D frontSensorHit;
    RaycastHit2D backSensorHit;

    [SerializeField] private Transform _frontSensorMainPos;
    [SerializeField] private Transform _frontSensor;
    [SerializeField] private Transform _backSensorMainPos;
    [SerializeField] private Transform _backSensor;

    private void OnEnable()
    {
        _isEnemyDead = false;
        _speed = 4.08f;
    }

    private void Start()
    {
        if (GameObject.Find("Player") != null)
            _player = GameObject.Find("Player").GetComponent<Player>();

        _enemyAnimator = GetComponent<Animator>();
        _enemyDestroyedTriggerID = Animator.StringToHash("EnemyDestroyed");

        _audioSource = GetComponent<AudioSource>();

        _boxCollider2D = GetComponent<BoxCollider2D>();

        _enemyContainer = GameObject.Find("[--ENEMIES CONTAINER--]");

        _spawnSystem = GameObject.Find("WaveSystem").GetComponent<SpawnSystem>();
    }

    private void Update()
    {
        EnemyMovement(this.transform);
    }

    private void FixedUpdate()
    {
        frontSensorHit = Physics2D.Raycast(_frontSensorMainPos.position,_frontSensor.position);
        backSensorHit = Physics2D.Raycast(_backSensorMainPos.position, _backSensor.position);

        Debug.DrawLine(transform.position, _frontSensor.position, Color.green);

        if (frontSensorHit.collider != null)
        {
            if (frontSensorHit.collider.GetComponent<Collectable>() != null)
                EnemyShooting();
        }

        if (backSensorHit.collider != null)
            if (backSensorHit.collider.CompareTag("Player"))
            {
                EnemyShootingToPlayer();
                Debug.Log(backSensorHit.collider.tag);
            }
                //emyShootingToPlayer();
    }

    void EnemyMovement(Transform objectToMove)
    {
        objectToMove.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.y <= -5.50f)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7.5f, 0);
        }
    }

    void EnemyShooting()
    {
        Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity, _enemyContainer.transform);
    }

    void EnemyShootingToPlayer()
    {
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity, _enemyContainer.transform);
        enemyLaser.GetComponent<Laser>().LaserStatus();
        enemyLaser.GetComponent<SpriteRenderer>().flipY = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.DamagePlayerLives();
            }
            OnEnemyDeath();
        }

        if (other.CompareTag("Laser"))
        {
            DamageByWeapon(other.gameObject);
        }

        if (other.CompareTag("HeatSeekingMissel"))
        {
            DamageByWeapon(other.gameObject);
        }
    }

    void DamageByWeapon(GameObject weaponObject)
    {
        _player.AddPointsToScore();
        Destroy(weaponObject.gameObject);
        OnEnemyDeath();
    }
    void OnEnemyDeath()
    {
        _isEnemyDead = true;
        _audioSource.PlayOneShot(_explotionEffectAudioClip);
        _boxCollider2D.enabled = false;
        _speed = 0;
        //Invoke("SetActiveFalseToEnemy", 5f);
        SetActiveFalseToEnemy();
    }

    void SetActiveFalseToEnemy()
    {
        _spawnSystem.ObjectWaveCheck();
        //_enemyAnimator.SetInteger("EnemyStatus", 0);
        _boxCollider2D.enabled = true;
        this.gameObject.SetActive(false);
    }
}
