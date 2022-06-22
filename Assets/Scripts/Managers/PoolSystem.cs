using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoolSystem : MonoBehaviour
{
    [Header("Container reference to hold our objects")]
    [SerializeField] private GameObject _objectsContainer;

    private int _poolObjectsSize = 30;

    private Dictionary<int, List<GameObject>> _objectDictionary = new Dictionary<int, List<GameObject>>();

    private WaveSystemManager _waveSystemManager;

    private void Start()
    {
        _waveSystemManager = GetComponent<WaveSystemManager>();
        CreateListOfObjects();
        GeneratePool();
    }

    private void CreateListOfObjects()
    {
        for (int i = 0; i < _waveSystemManager.WavesReferenceLenght(); i++)
        {
            for (int w = 0; w < _waveSystemManager.ObjectOnTheWaveLenght(i); w++)
            {
                var isObjectIdOnTheDictionary = _objectDictionary.ContainsKey(_waveSystemManager.IdOfTheObject(i, w));
                if (isObjectIdOnTheDictionary == false)
                    _objectDictionary.Add(_waveSystemManager.IdOfTheObject(i, w), new List<GameObject>());
            }
        }
    }

    private void GeneratePool()
    {
        for (int i = 0; i < _waveSystemManager.WavesReferenceLenght(); i++)
        {
            for (int w = 0; w < _waveSystemManager.ObjectOnTheWaveLenght(i); w++)
            {
                for (int p = 0; p < _poolObjectsSize; p++)
                {
                    if (_objectDictionary[_waveSystemManager.IdOfTheObject(i,w)].Count != _poolObjectsSize)
                    {
                        GameObject newObject = Instantiate(_waveSystemManager.GetGameObjectFromWave(i, w));
                        newObject.transform.SetParent(_objectsContainer.transform);
                        newObject.SetActive(false);
                        _objectDictionary[_waveSystemManager.IdOfTheObject(i, w)].Add(newObject);
                    }
                }
            }
        }
    }

    public GameObject RequestObjectToSpawn(int objectId)
    {
        var getObject = _objectDictionary[objectId].FirstOrDefault((obj) => obj.activeInHierarchy == false);

        if (getObject != null)
            return getObject;

        return null;
    }
}
