using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveSetUp
{
    public int objectID;
    public GameObject gameObjectToSpawn;
}

[CreateAssetMenu(menuName ="Wave System/Create New Wave")]
public class WaveSystemSO : ScriptableObject
{
    public bool _willThisBeARandomWave = false;

    [Header("Type of object on the wave")]
    public WaveSetUp[] objectToSpawnOnThisWave;

    [Header("Amount of objects to spawn on this wave")]
    public int amountToSpawnOnThisWave;
    private int _amountToSpawnOnThisWaveCounter = 0;

    public int ReturnObjectType()
    {
        if(_willThisBeARandomWave == true)
        {
            var nextObjectType = objectToSpawnOnThisWave[Random.Range(0, objectToSpawnOnThisWave.Length)].objectID;
            return nextObjectType;
        }
        else
        {
            for (int i = 0; i < objectToSpawnOnThisWave.Length; i++)
            {
                var nextObjectTypeByOrder = objectToSpawnOnThisWave[i].objectID;
                return nextObjectTypeByOrder;
            }
        }

        return 0;
    }
}
