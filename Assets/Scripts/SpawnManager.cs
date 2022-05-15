using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Enemies Variable Section
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemiesContainer;

    [SerializeField] private float _secondsToSpawn = 2.5f;
    [SerializeField] private bool _isPlayerDead = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (_isPlayerDead == false)
        {
            float _randomX = Random.Range(-8, 8);
            Vector3 _positionToSpawn = new Vector3(_randomX, 7.5f, 0);

            Instantiate(_enemyPrefab, _positionToSpawn, Quaternion.identity, _enemiesContainer.transform);

            yield return new WaitForSeconds(_secondsToSpawn);
        }
    }

    public void IsPlayerDead(bool playerStatus)
    {
        _isPlayerDead = playerStatus;
    }
}
