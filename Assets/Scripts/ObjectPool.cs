using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    
    [SerializeField] private int _poolSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> _poolDictionary = new();
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeNewObjectPool(GameObject prefab)
    {
        _poolDictionary[prefab] = new Queue<GameObject>();
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewObject(prefab);
        }
    }

    private void CreateNewObject(GameObject prefab)
    {
        var newObject = Instantiate(prefab, transform);
        
        newObject.AddComponent<PulledObject>().OriginalPrefab = prefab;
        
        newObject.SetActive(false);
        _poolDictionary[prefab].Enqueue(newObject);
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (_poolDictionary.ContainsKey(prefab) == false)
        {
            InitializeNewObjectPool(prefab);
        }

        if (_poolDictionary[prefab].Count == 0)
        {
            CreateNewObject(prefab);
        }
        
        GameObject objectToGet = _poolDictionary[prefab].Dequeue();
        objectToGet.SetActive(true);
        objectToGet.transform.parent = null;
        return objectToGet;
    }

    public void ReturnObject(GameObject objectToReturn, float delay = 0.001f)
    {
        StartCoroutine(ReturnObjectToPoolAfterDelay(objectToReturn, delay));
    }

    private IEnumerator ReturnObjectToPoolAfterDelay(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnObjectToPool(objectToReturn);
    }

    private void ReturnObjectToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        GameObject originalPrefab = objectToReturn.GetComponent<PulledObject>().OriginalPrefab;
        _poolDictionary[originalPrefab].Enqueue(objectToReturn);
        objectToReturn.transform.parent = transform;
    }
}
