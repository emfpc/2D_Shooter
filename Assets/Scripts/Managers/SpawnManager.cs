using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("GameObject List to Spawn Objects")]
    [SerializeField] private List<GameObject> _objectPrefab = new List<GameObject>();
    [SerializeField] private GameObject _objectContainer;

    [Header("Time estimated to Spawn the Objects")]
    [SerializeField] private float _secondsToSpawnObject;
    private WaitForSeconds _waitForSecondsToSpawnObject;

    private bool _isPlayerDead = false;

    private void OnEnable()
    {
        Player.OnGetIsPlayerDead += IsPlayerDead;
    }

    private void Start()
    {
        _waitForSecondsToSpawnObject = new WaitForSeconds(_secondsToSpawnObject);
    }

    public void StartSpawning()
    {
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

    private void OnDisable()
    {
        Player.OnGetIsPlayerDead -= IsPlayerDead;
    }
}
