using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Enemies Variable Section
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemiesContainer;

    [SerializeField] private float _secondsToSpawn = 2.5f;
    private WaitForSeconds _waitForSecondsToSpawn;
    [SerializeField] private bool _isPlayerDead = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        _waitForSecondsToSpawn = new WaitForSeconds(_secondsToSpawn);
    }

    IEnumerator SpawnEnemy()
    {
        while (_isPlayerDead == false)
        {
            float _randomX = Random.Range(-8, 8);
            Vector3 _positionToSpawn = new Vector3(_randomX, 7.5f, 0);

            Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity, _enemiesContainer.transform);

            yield return _waitForSecondsToSpawn;
        }
    }

    public void IsPlayerDead(bool playerStatus)
    {
        _isPlayerDead = playerStatus;
    }
}
