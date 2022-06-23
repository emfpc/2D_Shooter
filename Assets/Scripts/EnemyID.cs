using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID
{
    Enemy,
    SpinEnemy,
    EnemyAvoidShot,
    EnemyFighter,
    EnemySensor,
    EnemyBoss
}

public class EnemyID : MonoBehaviour
{
    [SerializeField] private ID _enemyID;
}
