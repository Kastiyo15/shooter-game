using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _player;
    [SerializeField] private float _threshold;


    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = (_player.position + mousePos) / 2f;

            targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _player.position.x, _threshold + _player.position.x);
            targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _player.position.y, _threshold + _player.position.y);
            targetPos.z = 0f;

            // Rotate the crosshair for fun
            transform.Rotate(Vector3.forward * 100f * Time.deltaTime);

            this.transform.position = targetPos;
        }
        return;
    }
}
