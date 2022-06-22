using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] protected float _speed = 3f;
    protected Player _player;

    private bool _isPlayerCallingForCollectables = false;
    private Transform _moveToPlayer;

    private void OnEnable()
    {
        Player.OnGetPlayerCallingForPowerUps += PlayerCallingForCollectable;
    }
    private void Start()
    {
        if(GameObject.Find("Player") != null)
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if(_isPlayerCallingForCollectables == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveToPlayer.position, 10 * Time.deltaTime);
        }else if(_isPlayerCallingForCollectables == false)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= -5.50f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void PlayerCallingForCollectable(bool collectableToMoveStatus, Transform nextPos)
    {
        _isPlayerCallingForCollectables = collectableToMoveStatus;
        _moveToPlayer = nextPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            ActivatePowerUp();

        if (other.CompareTag("Torpedo"))
            Destroy(this.gameObject);
    }

    protected virtual void ActivatePowerUp()
    {
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        Player.OnGetPlayerCallingForPowerUps -= PlayerCallingForCollectable;
    }
}
