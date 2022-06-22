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
    public bool _thisEnemyIsABoss = false;

    [Header("Type of object on the wave")]
    public WaveSetUp[] objectToSpawnOnThisWave;

    [Header("Amount of objects to spawn on this wave")]
    public int amountToSpawnOnThisWave;

    public int ReturnObjectType()
    {
        var nextObjectType = objectToSpawnOnThisWave[Random.Range(0, objectToSpawnOnThisWave.Length)].objectID;
        return nextObjectType;
    }
}
