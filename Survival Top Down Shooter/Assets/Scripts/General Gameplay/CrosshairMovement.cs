using UnityEngine;


public class CrosshairMovement : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _radius;

    private Vector3 _mousePos;
    private Vector3 _playerPos;
    private Vector3 _finalPos;


    void Awake()
    {
        _cam = Camera.main;
    }


    // Update is called once per frame
    void Update()
    { 
        _mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _playerPos = _player.transform.root.position;

        LimitCursor();
    }


    // Make use of functions from Update()
    void FixedUpdate()
    {
        // Smooth movement
        transform.position = Vector3.Slerp(transform.position, _finalPos, _speed * Time.fixedDeltaTime);

        // Rotate the crosshair for fun
        transform.Rotate(Vector3.forward * _speed * Time.fixedDeltaTime);

        // transform.position = Vector3.Lerp(this.transform.position, _mousePos, _speed * Time.deltaTime);
    }


    // Limit cursor to around player
    void LimitCursor()
    {
        Vector3 playerToCursor = _mousePos - _playerPos;    // Get vector from the player to the cursor
        Vector3 dir = playerToCursor.normalized;            // Normalize it to get the direction
        Vector3 cursorVector = dir * _radius;               // Multiply direction by your desired radius
        _finalPos = _playerPos + cursorVector;              // Add the cursor vector to the player position to get the final position
        _finalPos.z = 0f;                                   // Lock the z axis to 0
    }
}
