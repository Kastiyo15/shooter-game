using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool current;
    public GameObject PooledObject;
    public int PooledAmount;
    public bool willGrow;

    private List<GameObject> _pooledObjects;


    private void Awake()
    {
        // An instance of the object pooler
        current = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _pooledObjects = new List<GameObject>();

        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < PooledAmount; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            GameObject obj = Instantiate(PooledObject);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }


    public GameObject GetPooledObject()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        // Check if list will be dynamic
        if (willGrow)
        {
            GameObject obj = Instantiate(PooledObject);
            _pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
