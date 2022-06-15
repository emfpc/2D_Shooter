using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HeatSeekingMissele : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private GameObject[] _enemyShipsObject;
    private WaitForSeconds _heatMisseleWaitForSecondsStandby;
    private float _heatMisseleSecondStandby = 0.5f;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();

        if (_enemyShipsObject == null)
            Debug.Log("Something is wrong with the array");

        _heatMisseleWaitForSecondsStandby = new WaitForSeconds(_heatMisseleSecondStandby);
    }
    private void Update()
    {
        FindNearestEnemy();
    }

    IEnumerator FindTheNearestEnemy()
    {
        while (true)
        {
            _enemyShipsObject = GameObject.FindGameObjectsWithTag("Enemy");
            _enemyShipsObject = _enemyShipsObject.OrderBy((en) => Vector3.Distance(en.transform.position, _player.position)).ToArray();
            transform.position = Vector3.MoveTowards(transform.position, _enemyShipsObject[0].transform.position, 8 * Time.deltaTime);
            
            yield return _heatMisseleWaitForSecondsStandby;
        }
    }
    void FindNearestEnemy()
    {
        _enemyShipsObject = GameObject.FindGameObjectsWithTag("Enemy");
        _enemyShipsObject = _enemyShipsObject.OrderBy((en) => Vector3.Distance(en.transform.position, transform.position)).ToArray();
        transform.position = Vector3.MoveTowards(transform.position, _enemyShipsObject[0].transform.position, 12 * Time.deltaTime);
    }
}
