using System.Collections;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] private AnimationCurve _curve;
    public bool _start = false;


    // Update is called once per frame
    void Update()
    {
        if (_start)
        {
            _start = false;
            StartCoroutine(Shaking());
        }
    }


    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = _curve.Evaluate(elapsedTime / _duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
