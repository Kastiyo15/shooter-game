using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> PooledObjects;
    public GameObject ObjectToPool;
    public int AmountToPool;

    public bool willGrow;


    private void Awake()
    {
        // An instance of the object pooler
        SharedInstance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        PooledObjects = new List<GameObject>();
        GameObject tmp;

        // For loop,instantiate objects in the pooled amount, but keep a reference of them
        for (int i = 0; i < AmountToPool; i++)
        {
            // Create gameobjects, but set them inactive, add them to the list
            tmp = Instantiate(ObjectToPool);
            tmp.SetActive(false);
            PooledObjects.Add(tmp);
        }
    }


    public GameObject GetPooledObject()
    {
        // find an inactive object and return in, by looping through list
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }

        // Check if list will be dynamic
        if (willGrow)
        {
            GameObject obj = Instantiate(ObjectToPool);
            PooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
