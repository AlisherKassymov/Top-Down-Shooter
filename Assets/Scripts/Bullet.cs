using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletLifeTime;
    private Rigidbody _rb => GetComponent<Rigidbody>();

    private void Update()
    {
        if (_bulletLifeTime <= 0 && gameObject.activeSelf)
        {
            _bulletLifeTime = 5;
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
        _bulletLifeTime -= Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        GetComponent<TrailRenderer>().Clear();
        ObjectPool.Instance.ReturnBullet(gameObject);
    }
}
