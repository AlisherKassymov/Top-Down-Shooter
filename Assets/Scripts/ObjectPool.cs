using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize = 10;

    private Queue<GameObject> _bulletsPool;
    

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

    private void Start()
    {
        _bulletsPool = new Queue<GameObject>();
        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var newBullet = Instantiate(_bulletPrefab, transform);
            newBullet.SetActive(false);
            _bulletsPool.Enqueue(newBullet);
        }
    }

    public GameObject GetBullet()
    {
        GameObject bulletToGet = _bulletsPool.Dequeue();
        bulletToGet.SetActive(true);
        bulletToGet.transform.parent = null;
        return bulletToGet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        _bulletsPool.Enqueue(bullet);
        bullet.transform.parent = transform;
    }
    
}
