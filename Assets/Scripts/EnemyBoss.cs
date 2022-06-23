using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private bool _isEnemyBossArrivedAtDestination = false;
    private bool _isEnemyMovingToTheLeft = false;
    [SerializeField] private GameObject _enemyBossBulletPrefab;

    [SerializeField] private List<Transform> _transformListToSpawn = new List<Transform>();
    [SerializeField]
    private int _enemyBossLives = 20;

    //Shooting Variable Section
    private float _firingRate = 3.0f;
    private float _canIFire = -1f;

    private void Start()
    {
        transform.position = new Vector3(0, 30f, 0);
    }
    private void Update()
    {
        if (_isEnemyBossArrivedAtDestination == false)
            EnemyBossPrimaryMovement();

        if (_isEnemyBossArrivedAtDestination == true)
            EnemyBossMainMovement();
    }

    void EnemyBossPrimaryMovement()
    {
        transform.Translate(Vector3.down * 5f * Time.deltaTime);
        if (transform.position.y <= 8)
            _isEnemyBossArrivedAtDestination = true;
    }

    void EnemyBossMainMovement()
    {
        if (_isEnemyMovingToTheLeft == false)
            EnemyBossMoveRight();

        if (_isEnemyMovingToTheLeft == true)
            EnemyBossMoveLeft();

        Invoke("EnemyBossShooting", 2f);

        //EnemyBossShooting();
    }

    void EnemyBossMoveRight()
    {
        transform.Translate(Vector3.right * 3f * Time.deltaTime);
        if (transform.position.x >= 5)
            _isEnemyMovingToTheLeft = true;
    }

    void EnemyBossMoveLeft()
    {
        transform.Translate(-Vector3.right * 3f * Time.deltaTime);
        if (transform.position.x <= -5)
            _isEnemyMovingToTheLeft = false;
    }

    void EnemyBossShooting()
    {
        int randomIndex = Random.Range(0, _transformListToSpawn.Count);
        if (Time.time > _canIFire)
        {
            _firingRate = Random.Range(3f, 4f);
            _canIFire = Time.time + _firingRate;
            Instantiate(_enemyBossBulletPrefab, _transformListToSpawn[randomIndex].position, Quaternion.identity);
        }
                
        //Instantiate(_enemyBossBulletPrefab, _transformListToSpawn[randomIndex].position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
            EnemyDamage();
    }

    void EnemyDamage()
    {
        if(_enemyBossLives < 1)
        {
            Destroy(this.gameObject);
            return;
        }
        _enemyBossLives--;
    }
}
