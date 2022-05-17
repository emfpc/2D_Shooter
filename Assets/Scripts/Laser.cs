using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 7.5f;

    // Update is called once per frame
    void Update()
    {
        MoveUpLaser();
        DestroyLaser();
    }

    void MoveUpLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    void DestroyLaser()
    {
        if(transform.position.y > 8f)
        {
            if (transform.parent.CompareTag("TripleShot"))
                Destroy(transform.parent.gameObject);

            Destroy(this.gameObject);
        }
    }
}
