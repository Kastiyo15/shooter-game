using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 2f;
    [SerializeField] private Transform _target;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(_target.position.x, _target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, _followSpeed * Time.deltaTime);
    }
}
