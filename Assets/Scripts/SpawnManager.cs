using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _objectPrefab = new List<GameObject>();
    [SerializeField] protected GameObject _objectContainer;
        
    [SerializeField] protected float _secondsToSpawnObject;
    private WaitForSeconds _waitForSecondsToSpawnObject;
    private bool _isPlayerDead = false;

    private void Start()
    {
        _waitForSecondsToSpawnObject = new WaitForSeconds(_secondsToSpawnObject);
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while(_isPlayerDead == false)
        {
            float _randomX = Random.Range(-8, 8);
            Vector3 _positionToSpawn = new Vector3(_randomX, 7.5f, 0);
            int _randomIndex = Random.Range(0, _objectPrefab.Count);

            Instantiate(_objectPrefab[_randomIndex], _positionToSpawn, Quaternion.identity, _objectContainer.transform);

            yield return _waitForSecondsToSpawnObject;
        }
    }
    
    public void IsPlayerDead(bool playerStatus)
    {
        _isPlayerDead = playerStatus;
    }
}
