using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] float _buffer;
    [SerializeField] Vector3 centre;


    void Awake()
    {
        Camera _cam = Camera.main;
    }


    void Update()
    {
        var (centre, size) = CalculateOrthoSize();
        _cam.transform.position = centre;
        _cam.orthographicSize = size;
    }


    private (Vector3 centre, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();

        foreach (var col in FindObjectsOfType<Collider2D>()) bounds.Encapsulate(col.bounds);

        bounds.Expand(_buffer);

        var vertical = bounds.size.y;
        var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

        var size = Mathf.Max(horizontal, vertical) * 0.5f;

        return (centre, size);
    }
}
