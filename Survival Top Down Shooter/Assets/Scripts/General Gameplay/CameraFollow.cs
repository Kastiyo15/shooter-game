using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 2f;
    [SerializeField] private Transform _target;
    [SerializeField] private float _radius;
    [SerializeField] private int _boundary = 250;


    // Update is called once per frame
    void Update()
    {
        var targetX = _target.position.x;
        var targetY = _target.position.y;

        Vector3 newPos = new Vector3(targetX, targetY, -10f);


        // Move the camera to player if player moves out of a radius on the x-axis
        if (targetX > transform.position.x + _radius || targetX < transform.position.x - _radius)
        {
            InitialiseFollow(newPos);
        }

        // Move the camera to player if player moves out of a radius on the y-axis
        if (targetY > transform.position.y + _radius || targetY < transform.position.y - _radius)
        {
            InitialiseFollow(newPos);
        }
    }


    // Function to move position of the camera
    private void InitialiseFollow(Vector3 newPos)
    {
        // Limit the movement of the camera to a 500 x 500 grid
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -_boundary, _boundary);
        pos.y = Mathf.Clamp(transform.position.y, -_boundary, _boundary);
        transform.position = Vector3.Slerp(pos, newPos, _followSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}
