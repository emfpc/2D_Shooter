using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnManager : MonoBehaviour
{
    [Header("GameObject List to Spawn Objects")]
    [SerializeField] private List<GameObject> _objectPrefab = new List<GameObject>();
    [SerializeField] private GameObject _objectContainer;

    [Header("Time estimated to Spawn the Objects")]
    [SerializeField] private float _secondsToSpawnObject;
    private WaitForSeconds _waitForSecondsToSpawnObject;

    private bool _isPlayerDead = false;

    [Header("Rare Object to Spawn")]
    [HideInInspector] public bool _doseSpawnerHasRareObjectToSpawn;
    [HideInInspector] public List<GameObject> _rareObjectPrefab = new List<GameObject>();
    [HideInInspector] public float _secondsToSpawnRareObject;
    private WaitForSeconds _waitForSecondsToSpawnRareObject;

    private void OnEnable()
    {
        Player.OnGetIsPlayerDead += IsPlayerDead;
    }

    private void Start()
    {
        _waitForSecondsToSpawnObject = new WaitForSeconds(_secondsToSpawnObject);
        _waitForSecondsToSpawnRareObject = new WaitForSeconds(_secondsToSpawnRareObject);
    }
    public void CallingToStartSpawning()
    {
        Invoke("StartSpawning", 3f);
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnObjects());

        if (_doseSpawnerHasRareObjectToSpawn == true)
            StartCoroutine(SpawnRareObjects());
    }

    IEnumerator SpawnObjects()
    {
        while(_isPlayerDead == false)
        {
            float _randomX = UnityEngine.Random.Range(-8, 8);
            Vector3 _positionToSpawn = new Vector3(_randomX, 7.5f, 0);
            int _randomIndex = UnityEngine.Random.Range(0, _objectPrefab.Count);

            Instantiate(_objectPrefab[_randomIndex], _positionToSpawn, Quaternion.identity, _objectContainer.transform);

            yield return _waitForSecondsToSpawnObject;
        }
    }

    IEnumerator SpawnRareObjects()
    {
        while (_isPlayerDead == false)
        {
            float _randomX = UnityEngine.Random.Range(-8, 8);
            Vector3 _positionToSpawn = new Vector3(_randomX, 7.5f, 0);
            int _randomIndex = UnityEngine.Random.Range(0, _rareObjectPrefab.Count);

            Instantiate(_rareObjectPrefab[_randomIndex], _positionToSpawn, Quaternion.identity, _objectContainer.transform);

            yield return _waitForSecondsToSpawnObject;
        }
    }

    
    public void IsPlayerDead(bool playerStatus)
    {
        _isPlayerDead = playerStatus;
    }

    public void DoseSpawnerHasRareObjectToSpawn(bool rareObjectStatus)
    {
        _doseSpawnerHasRareObjectToSpawn = rareObjectStatus;
    }

    private void OnDisable()
    {
        Player.OnGetIsPlayerDead -= IsPlayerDead;
    }
}