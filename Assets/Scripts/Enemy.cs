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

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _enemyAnimator = GetComponent<Animator>();
        _enemyDestroyedTriggerID = Animator.StringToHash("EnemyDestroyed");

        _boxCollider2D = GetComponent<BoxCollider2D>();

    }
    private void Update()
    {
        EnemyMovement();
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
            _player.AddPointsToScore();
            Destroy(other.gameObject);
            OnEnemyDeath();
        }
    }

    void OnEnemyDeath()
    {
        _speed = 0f;
        _enemyAnimator.SetTrigger(_enemyDestroyedTriggerID);
        _boxCollider2D.enabled = false;

        Destroy(this.gameObject, 5f);
    }
}
