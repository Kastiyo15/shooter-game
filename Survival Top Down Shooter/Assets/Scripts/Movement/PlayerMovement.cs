using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Camera _cam;
    [SerializeField] float _speed;

    private Vector2 _moveVelocity;
    private Vector2 _mousePos;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cam = Camera.main;

        PauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
    }


    // Update is called once per frame
    void Update()
    {
        // Update a variable based on players move inputs
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveVelocity = moveInput.normalized * _speed;

        // Get position of the mouse
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    }


    // Used to run physics
    void FixedUpdate()
    {
        // Move player based on the move inputs variable from Update()
        _rb.MovePosition(_rb.position + _moveVelocity * _speed * Time.deltaTime);

        // Rotate player based on where the mouse is looking
        Vector2 lookDir = _mousePos - _rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = angle;
    }
}
