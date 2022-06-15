using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.8f;
    private Player _player;

    private Animator _enemyAnimator;
    private int _enemyDestroyedTriggerID;

    private BoxCollider2D _boxCollider2D;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explotionEffectAudioClip;

    //Shooting Variable Section
    private float _firingRate= 3.0f;
    private float _canIFire= -1f;

    //Enemy Laser Object Variable Section
    [Header("Enemy Laser object field")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    private GameObject _enemyContainer;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _enemyAnimator = GetComponent<Animator>();
        _enemyDestroyedTriggerID = Animator.StringToHash("EnemyDestroyed");

        _boxCollider2D = GetComponent<BoxCollider2D>();

        _audioSource = GetComponent<AudioSource>();

        _enemyContainer = GameObject.Find("[--ENEMIES CONTAINER--]");

    }
    private void Update()
    {
        EnemyMovement();
        EnemyShooting();
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5.50f)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7.5f, 0);
        }
    }

    void EnemyShooting()
    {
        if(Time.time > _canIFire)
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
            if(_player != null)
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
        _audioSource.PlayOneShot(_explotionEffectAudioClip);
        _speed = 0f;
        _enemyAnimator.SetTrigger(_enemyDestroyedTriggerID);
        _boxCollider2D.enabled = false;

        Destroy(this.gameObject, 5f);
    }
}
