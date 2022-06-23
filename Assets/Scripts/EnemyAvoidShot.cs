using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidShot : MonoBehaviour
{
    [SerializeField] private float _speed = 4.8f;
    private Player _player;

    private BoxCollider2D _boxCollider2D;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explotionEffectAudioClip;

    //Shooting Variable Section
    private float _firingRate = 3.0f;
    private float _canIFire = -1f;

    //Enemy Laser Object Variable Section
    [Header("Enemy Laser object field")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    private GameObject _enemyContainer;

    private SpawnSystem _spawnSystem;

    private bool _isEnemyDead = false;

    [SerializeField] private EnemySensor _enemySensor;

    private void OnEnable()
    {
        _speed = 4.8f;
    }
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();

        _boxCollider2D = GetComponent<BoxCollider2D>();

        _enemyContainer = GameObject.Find("[--ENEMIES CONTAINER--]");

        _spawnSystem = GameObject.Find("WaveSystem").GetComponent<SpawnSystem>();
    }
    private void Update()
    {
        EnemyMovement(this.transform);
        EnemyShooting();
    }

    void EnemyMovement(Transform objectToMove)
    {
        if(_enemySensor.WillEnemyEvadeShotStatus() == true)
        {
            objectToMove.Translate(Vector3.down * 10 * Time.deltaTime);
            Invoke("ChangeEvadeStatus", 2.5f);
            return;
        }
        objectToMove.Translate(-Vector3.right * _speed * Time.deltaTime);

        if (transform.position.y <= -5.50f)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7.5f, 0);
        }
    }

    void ChangeEvadeStatus()
    {
        _enemySensor.WillEnemyEvadeShotChangeStatus(false);
    }

    void EnemyShooting()
    {
        if (Time.time > _canIFire && _isEnemyDead == false)
        {
            _firingRate = Random.Range(3f, 7f);
            _canIFire = Time.time + _firingRate;
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity, _enemyContainer.transform);
        }
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
        SetActiveFalseToEnemy();
        //Invoke("SetActiveFalseToEnemy");
    }

    void SetActiveFalseToEnemy()
    {
        _spawnSystem.ObjectWaveCheck();
        _boxCollider2D.enabled = true;
        this.gameObject.SetActive(false);
    }
}
