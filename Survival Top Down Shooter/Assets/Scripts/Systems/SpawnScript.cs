using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnScript : MonoBehaviour
{


    [Header("Spawn Points")]
    [SerializeField] private float _rangeX;
    [SerializeField] private float _rangeY;
    [SerializeField] private float _playerX;
    [SerializeField] private float _playerY;

    [Header("Variables")]
    [SerializeField] private float _spawnInterval;
    [SerializeField] private int _spawnMax;
    public int SpawnCount = 0;

    [Header("References")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private KillCounter _killCount;


    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //Instantiate(_playerPrefab);

        StartCoroutine(spawnEnemy(_spawnInterval, _enemyPrefab));
    }


    private void FindNewPosition()
    {
        _rangeX = Mathf.Floor(mainCamera.orthographicSize * mainCamera.aspect);
        _playerX = mainCamera.transform.position.x;
        _rangeY = Mathf.Floor(mainCamera.orthographicSize);
        _playerY = mainCamera.transform.position.y;
    }


    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        FindNewPosition();

        if (/* SpawnCount < _spawnMax && */ _player.activeInHierarchy)
        {
            GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range((_playerX - _rangeX), (_playerX + _rangeX)), Random.Range((_playerY - _rangeY), (_playerY + _rangeY)), 0), Quaternion.identity);
            IncreaseSpawnCount();
            StartCoroutine(spawnEnemy(interval, enemy));
        }
    }


    private void IncreaseSpawnCount()
    {
        SpawnCount++;
    }
}
