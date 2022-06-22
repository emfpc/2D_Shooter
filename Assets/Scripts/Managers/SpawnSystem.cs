using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [Header("Waves properties")]
    private int _waveNumber;
    private int _currentObjectToSpawnOnScene;
    private int _objectDestroyed = 0;

    [Header("Wait seconds between object spawn")]
    private WaitForSeconds _objectToSpawnWaitForSeconds;
    [SerializeField] private float _waitForSecondsBetweenObjectSpawn = 3f;

    [Header("Wait seconds between waves")]
    private WaitForSeconds _nextWaveRoutineWaitForSeconds;
    [Range(0, 10f)]
    [SerializeField] private float _waitForSecondsBetweenWave = 3f;

    [Header("Position / destination where the objects are going to spawn")]
    [SerializeField] private Vector3 _objectPosition;

    private WaveSystemManager _waveSystemManager;
    private PoolSystem _poolSystem;
    private UIManager _uiManager;

    private void Start()
    {
        _waveSystemManager = GetComponent<WaveSystemManager>();
        _poolSystem = GetComponent<PoolSystem>();
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        _objectToSpawnWaitForSeconds = new WaitForSeconds(_waitForSecondsBetweenObjectSpawn);
        _nextWaveRoutineWaitForSeconds = new WaitForSeconds(_waitForSecondsBetweenWave);
    }

    public void StartWave()
    {
        StartObjectWave();
    }

    private void StartObjectWave()
    {
        _uiManager.WaveTextIndicator(_waveNumber + 1);
        StartCoroutine(NextWaveRoutine());
    }

    private IEnumerator NextWaveRoutine()
    {
        yield return _nextWaveRoutineWaitForSeconds;
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        _currentObjectToSpawnOnScene = _waveSystemManager.AmountOfObjectToSpawnInThisWave(_waveNumber);

        for (int i = 0; i < _currentObjectToSpawnOnScene; i++)
        {
            var selectedObject = _waveSystemManager.ReturnObjectTypeId(_waveNumber);
            GameObject newObject = _poolSystem.RequestObjectToSpawn(selectedObject);

            newObject.transform.position = _objectPosition;
            newObject.SetActive(true);

            yield return _objectToSpawnWaitForSeconds;
        }
    }

    public void ObjectWaveCheck()
    {
        _objectDestroyed++;

        if(_objectDestroyed == _currentObjectToSpawnOnScene)
        {
            _objectDestroyed = 0;
            _waveNumber++;

            if((_waveNumber +1) > _waveSystemManager.WavesReferenceLenght())
            {
                _uiManager.WaveTextIndicator(0);
                _waveNumber = 0;
                _objectDestroyed = 0;
            }
            else
            {
                _uiManager.WaveTextIndicator((_waveNumber + 1));
                StartCoroutine(NextWaveRoutine());
            }
        }
    }
}
