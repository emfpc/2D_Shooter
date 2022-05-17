using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private float _speed = 3f;
    protected Player _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
            Debug.Log("Player is NULL :: PowerUps Script");
    }

    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.50f)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7.5f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp();
        }
    }

    protected virtual void ActivatePowerUp()
    {
        Destroy(this.gameObject);
    }
}
