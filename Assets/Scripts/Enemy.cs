using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.8f;
    private Player _player;

    private Animator _enemyAnimator;
    private int _enemyDestroyedTriggerID;

    private BoxCollider2D _boxCollider2D;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _explotionEffectAudioClip;

    //Shooting Variable Section
    private float _firingRate= 3.0f;
    private float _canIFire= -1f;

    //Enemy Laser Object Variable Section
    [Header("Enemy Laser object field")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    private GameObject _enemyContainer;

    //Phase II
    [HideInInspector] public bool _doseThisEnemyHasCircleMovement;
    [HideInInspector] public GameObject _enemyObjectParent;
    [HideInInspector] public GameObject _enemyObject;
    [HideInInspector] public int _wayPointsEnd = 10;
    [HideInInspector] public float _radiousSize = 10f;
    private List<Vector3> _wayPoints = new List<Vector3>();
    private int _indexWayPoints = 0;
    private SpawnSystem _spawnSystem;
    private bool _isEnemyDead = false;

    private bool _isEnemyBeingAggressive = false;
    [SerializeField] private GameObject _enemyShield;
    private bool _willEnemyHaveShield = false;
    private int _randomIndexForShield = 0;

    private void OnEnable()
    {
        _isEnemyDead = false;
        _speed = 4.08f;
    }
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _enemyAnimator = GetComponent<Animator>();
        _enemyDestroyedTriggerID = Animator.StringToHash("EnemyDestroyed");

        _audioSource = GetComponent<AudioSource>();

        _boxCollider2D = GetComponent<BoxCollider2D>();

        _enemyContainer = GameObject.Find("[--ENEMIES CONTAINER--]");

        _spawnSystem = GameObject.Find("WaveSystem").GetComponent<SpawnSystem>();

        CreateCirclePath(_wayPointsEnd, _radiousSize);

        WillEnemyHaveShield();
    }
    private void Update()
    {       

        if(_doseThisEnemyHasCircleMovement == true)
        {
            EnemyMovement(_enemyObjectParent.transform);
            EnemyCircleMovement();
        }
        else
        {
            EnemyMovement(this.transform);
            EnemyShooting();
            EnemyAggressiveBahaviour();
        }

    }

    private void WillEnemyHaveShield()
    {
        _randomIndexForShield = Random.Range(0, 2);

        switch (_randomIndexForShield)
        {
            case 0:
                _willEnemyHaveShield = false;
                _enemyShield.SetActive(false);
                break;
            case 1:
                _willEnemyHaveShield = true;
                _enemyShield.SetActive(true);
                break;

            default:
                break;
        }
    }

    void EnemyMovement(Transform objectToMove)
    {
        if (_isEnemyBeingAggressive == true)
            return;

        objectToMove.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5.50f)
        {
            float _randomX = Random.Range(-8, 8);
            transform.position = new Vector3(_randomX, 7.5f, 0);
        }
    }

    void EnemyShooting()
    {
        if(Time.time > _canIFire && _isEnemyDead == false)
        {
            _firingRate = Random.Range(3f, 7f);
            _canIFire = Time.time + _firingRate;
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity, _enemyContainer.transform);
        }
    }
    
    Vector3 EnemyCircleMovement()
    {
        float wayPointDistance = Vector3.Distance(_enemyObject.transform.position, _wayPoints[_indexWayPoints]);

        if (wayPointDistance <= 1)
        {
            _indexWayPoints++;
        }

        if (_indexWayPoints > _wayPoints.Count - 1)
        {
            _indexWayPoints = 0;
        }

        return _enemyObject.transform.position = Vector3.MoveTowards(_enemyObject.transform.position, _wayPoints[_indexWayPoints], 3 * Time.deltaTime);
    }

    void EnemyAggressiveBahaviour()
    {
        if(_player != null)
        {
            float distance = (transform.position - _player.transform.position).magnitude;
            if (distance <= 3)
            {
                _isEnemyBeingAggressive = true;
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 10 * Time.deltaTime);
            }
            else
            {
                _isEnemyBeingAggressive = false;
            }
               
            Debug.Log($"Player Distance {distance} :: EnemyScript");
        }
    }

    void CreateCirclePath(int points, float radius)
    {
        Vector3 startCorner = new Vector3(0, 0, 0);

        Vector3 previousCorner = startCorner;

        for (int i = 0; i < points; i++)
        {

            float cornerAngle = 2f * Mathf.PI / (float)points * i;

            Vector3 currentCorner = new Vector3(Mathf.Cos(cornerAngle) * radius, Mathf.Sin(cornerAngle) * radius, 0) + startCorner;

            _wayPoints.Add(currentCorner);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hello");
        if (other.CompareTag("Player"))
        {
            if(_player != null)
            {
                _player.DamagePlayerLives();
            }
            OnEnemyDeath();
        }

        if (other.CompareTag("Laser"))
        {
            DamageByWeapon(other.gameObject);
        }

        if (other.CompareTag("HeatSeekingMissel"))
        {
            DamageByWeapon(other.gameObject);
        }
    }

    void DamageByWeapon(GameObject weaponObject)
    {
        _player.AddPointsToScore();
        Destroy(weaponObject.gameObject);
        OnEnemyDeath();
    }
    void OnEnemyDeath()
    {
        if(_willEnemyHaveShield == true)
        {
            _isEnemyBeingAggressive = false;
            _willEnemyHaveShield = false;
            _enemyShield.SetActive(false);
            return;
        }

        _isEnemyDead = true;
        _audioSource.PlayOneShot(_explotionEffectAudioClip);
        //_enemyAnimator.SetTrigger(_enemyDestroyedTriggerID);
        _enemyAnimator.SetInteger("EnemyStatus", 1);
        _boxCollider2D.enabled = false;
        _speed = 0;
        
        //Destroy(this.gameObject, 5f);
        Invoke("SetActiveFalseToEnemy", 5f);
    }

    void SetActiveFalseToEnemy()
    {
        _spawnSystem.ObjectWaveCheck();
        _enemyAnimator.SetInteger("EnemyStatus", 0);

        if (transform.parent.CompareTag("SpinEnemy"))
        {
            //Destroy(transform.parent.gameObject, 5f);
            //Invoke("SetActiveFalseToEnemy", 5f);
            _boxCollider2D.enabled = true;
            transform.parent.gameObject.SetActive(false);
            return;
        }
        _boxCollider2D.enabled = true;
        this.gameObject.SetActive(false);
    }
}

[CustomEditor(typeof(Enemy))]
[CanEditMultipleObjects]
public class EnemyCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SerializedProperty _isEnemycircling;
        SerializedProperty _enemyObjectParent;
        SerializedProperty _enemyObject;
        SerializedProperty _wayPoints;
        SerializedProperty _radiusSize;

        _isEnemycircling = serializedObject.FindProperty("_doseThisEnemyHasCircleMovement");
        _enemyObjectParent = serializedObject.FindProperty("_enemyObjectParent");
        _enemyObject = serializedObject.FindProperty("_enemyObject");
        _wayPoints = serializedObject.FindProperty("_wayPointsEnd");
        _radiusSize = serializedObject.FindProperty("_radiousSize");

        EditorGUILayout.PropertyField(_isEnemycircling);

        if (_isEnemycircling.boolValue)
        {
            EditorGUILayout.PropertyField(_enemyObjectParent);
            EditorGUILayout.PropertyField(_enemyObject);
            EditorGUILayout.PropertyField(_wayPoints);
            EditorGUILayout.PropertyField(_radiusSize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
