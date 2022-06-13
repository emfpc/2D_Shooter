using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpawnManager[] _spawnManager;
    private CircleCollider2D _circleCollider2D;
    private Animator _asteroidAnimator;
    private int _asteroidExplotionAnimID;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explotionEffectAudioClip;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponents<SpawnManager>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _asteroidAnimator = GetComponent<Animator>();
        _asteroidExplotionAnimID = Animator.StringToHash("AsteroidExplotion");
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * 10 * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            foreach (var spawners in _spawnManager)
                spawners.CallingToStartSpawning();

            _audioSource.PlayOneShot(_explotionEffectAudioClip);
            _circleCollider2D.enabled = false;
            _asteroidAnimator.SetTrigger(_asteroidExplotionAnimID);
            Destroy(this.gameObject, 5f);
        }
    }
}
