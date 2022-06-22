using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBullets : MonoBehaviour
{
    private Transform _playerObj;

    private void Start()
    {
        if (GameObject.Find("Player") != null)
            _playerObj = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _playerObj.position, 5f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var _player = other.gameObject.GetComponent<Player>();
            _player.DamagePlayerLives();
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(this.gameObject);
        }
    }
}
