using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;


    // Pool Player Bullets
    [SerializeField] private GameObject _playerBullet;
    private List<GameObject> _playerBulletPool;
    [SerializeField] public int _playerBulletPoolSize;


    // Pool Player Bullets
    [SerializeField] private GameObject _enemyBullet;
    private List<GameObject> _enemyBulletPool;
    [SerializeField] private int _enemyBulletPoolSize;


    // Pool Player BulletEffect
    [SerializeField] private GameObject _playerBulletFX;
    private List<GameObject> _playerBulletFXPool;
    [SerializeField] private int _playerBulletFXPoolSize;


    // Pool Player BulletEffect
    [SerializeField] private GameObject _enemyBulletFX;
    private List<GameObject> _enemyBulletFXPool;
    [SerializeField] private int _enemyBulletFXPoolSize;


    // Boolean, grow list if true
    public bool willGrow;


    private void Awake()
    {
        if (SharedInstance == null)
        {
            // An instance of the object pooler
            SharedInstance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // PLAYER BULLETS // 
        _playerBulletPoolSize = StatsManager.Instance.BulletClipSize;

        _playerBulletPool = new List<GameObject>();
        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < _playerBulletPoolSize; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            GameObject tmp;
            tmp = Instantiate(_playerBullet, this.transform);
            tmp.SetActive(false);
            _playerBulletPool.Add(tmp);
        }

        // PLAYER BULLETS FX // 
        _playerBulletFXPool = new List<GameObject>();
        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < _playerBulletFXPoolSize; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            GameObject tmp;
            tmp = Instantiate(_playerBulletFX, this.transform);
            tmp.SetActive(false);
            _playerBulletFXPool.Add(tmp);
        }


        // ENEMY BULLETS // 
        _enemyBulletPool = new List<GameObject>();
        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < _enemyBulletPoolSize; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            GameObject tmp;
            tmp = Instantiate(_enemyBullet, this.transform);
            tmp.SetActive(false);
            _enemyBulletPool.Add(tmp);
        }

        // ENEMY BULLETS FX // 
        _enemyBulletFXPool = new List<GameObject>();
        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < _enemyBulletFXPoolSize; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            GameObject tmp;
            tmp = Instantiate(_enemyBulletFX, this.transform);
            tmp.SetActive(false);
            _enemyBulletFXPool.Add(tmp);
        }
    }


    // Player Functions
    public GameObject GetPlayerBulletFromPool()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < _playerBulletPoolSize; i++)
        {
            if (!_playerBulletPool[i].activeInHierarchy)
            {
                return _playerBulletPool[i];
            }
        }

        return null;
    }


    public GameObject GetPlayerBulletFXFromPool()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < _playerBulletFXPoolSize; i++)
        {
            if (!_playerBulletFXPool[i].activeInHierarchy)
            {
                return _playerBulletFXPool[i];
            }
        }


        // Check if list will be dynamic
        if (willGrow)
        {
            GameObject obj = Instantiate(_playerBulletFX);
            _playerBulletFXPool.Add(obj);
            return obj;
        }
        return null;
    }


    // Enemy Functions
    public GameObject GetEnemyBulletFromPool()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < _enemyBulletPoolSize; i++)
        {
            if (!_enemyBulletPool[i].activeInHierarchy)
            {
                return _enemyBulletPool[i];
            }
        }


        // Check if list will be dynamic
        if (willGrow)
        {
            GameObject obj = Instantiate(_enemyBullet);
            _enemyBulletPool.Add(obj);
            return obj;
        }
        return null;
    }


    public GameObject GetEnemyBulletFXFromPool()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < _enemyBulletFXPoolSize; i++)
        {
            if (!_enemyBulletFXPool[i].activeInHierarchy)
            {
                return _enemyBulletFXPool[i];
            }
        }


        // Check if list will be dynamic
        if (willGrow)
        {
            GameObject obj = Instantiate(_enemyBulletFX);
            _enemyBulletFXPool.Add(obj);
            return obj;
        }
        return null;
    }
}
