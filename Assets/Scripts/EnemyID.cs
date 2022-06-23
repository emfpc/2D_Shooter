using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ID
{
    Enemy,
    SpinEnemy,
    EnemyAvoidShot,
    EnemyFighter,
    EnemyDestroyer,
    EnemyBoss
}

public class EnemyID : MonoBehaviour
{
    [SerializeField] private ID _enemyID;
}
