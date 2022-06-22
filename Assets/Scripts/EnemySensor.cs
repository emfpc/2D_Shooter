using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    private bool _willEnemyEvadeShot = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") || other.CompareTag("HeatSeekingMissel"))
        {
            int randomeRange = Random.Range(0, 2);

            if (randomeRange == 0)
                _willEnemyEvadeShot = false;
            if (randomeRange == 1)
                _willEnemyEvadeShot = true;
        }
    }

    public bool WillEnemyEvadeShotStatus()
    {
        return _willEnemyEvadeShot;
    }

    public void WillEnemyEvadeShotChangeStatus(bool status)
    {
        _willEnemyEvadeShot = status;
    }
}
