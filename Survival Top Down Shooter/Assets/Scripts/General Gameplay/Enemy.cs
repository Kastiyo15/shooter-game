using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int _scoreValue;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed = 2f;
    private float moveDuration;
    private float pauseDuration;
    public float Timer;
    private bool _coolDown = false;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    private Transform _playerPos;
    private GameObject _player;


    // Called before Start
    void Awake()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    // Start is called before the first frame update
    void Start()
    {
        _speed = Mathf.Floor(Random.Range(_speed, _speed * 2));
        _rotationSpeed = Mathf.Floor(Random.Range(_rotationSpeed, _rotationSpeed * 2));

        // Enemy stop and go variables
        moveDuration = Random.Range(_speed, _speed * 1.5f);
        pauseDuration = 0.5f * moveDuration;
        Timer = moveDuration;

        // Move Enemy using Coroutine
        StartCoroutine(moveEnemy(pauseDuration));
    }


    void FixedUpdate()
    {
        // Rotate Enemy towards player
        #region RotateEnemy
        Vector2 playerPos = _playerPos.position;                                   // Make Enemy look at Player using vector maths
        Vector2 lookDir = playerPos - _rb.position;                             //
        float _angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;  // Rotate Enemy along the z axis, Atan2 finds radians/angle of x-axis to vector direction, convert radians to degrees, -90 degrees to offset it 
        _rb.rotation = _angle;                                                   // Apply this to our Enemy
        #endregion

    }


    //  Moving enemy when not on cooldown
    private IEnumerator moveEnemy(float paused)
    {
        // Start moving everytime this starts
        _coolDown = false;

        // Move Enemy when not coolDown
        while (!_coolDown && Timer > 0)
        {
            Timer -= Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
            HomingMovement();
            yield return null;
        }

        // Once finished make coolDown
        _coolDown = true;
        yield return new WaitForSeconds(paused);

        // Freeze velocity while coolDown
        while (_coolDown)
        {
            Timer = moveDuration;
            _rb.velocity = new Vector2(0, 0); // Freeze enemy position
                                              // Start again
            StartCoroutine(moveEnemy(pauseDuration));
        }
    }


    void HomingMovement()
    {
        var dir = _playerPos.position - transform.position;
        transform.up = Vector3.MoveTowards(transform.up, dir, _rotationSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, _speed * Time.deltaTime);
    }
}
