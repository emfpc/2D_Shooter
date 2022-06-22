using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 7.5f;
    [SerializeField] private bool _isThisAEnemyLaser = false;
    private bool _isThisATorpedo = false;
    // Update is called once per frame
    void Update()
    {
        if(_isThisAEnemyLaser == true)
        {
            MoveLaser(Vector3.down);
            DestroyEnemyLaser(-4f);
        }else if(_isThisAEnemyLaser == false)
        {
            MoveLaser(Vector3.up);
            DestroyPlayerLaser(8f);
        }
    }

    void MoveLaser(Vector3 vectorDirection)
    {
        transform.Translate(vectorDirection * _speed * Time.deltaTime);
    }

    void DestroyPlayerLaser(float floatBound)
    {
        if(transform.position.y > floatBound)
        {
            if (transform.parent.CompareTag("TripleShot"))
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }

    void DestroyEnemyLaser(float floatBound)
    {
        if (transform.position.y < floatBound)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && _isThisAEnemyLaser == true || _isThisATorpedo == true )
        {
            var player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.DamagePlayerLives();
                Destroy(this.gameObject);
            }
        }
    }

    public void LaserStatus()
    {
        _isThisAEnemyLaser = false;
        _isThisATorpedo = true;
    }
}
