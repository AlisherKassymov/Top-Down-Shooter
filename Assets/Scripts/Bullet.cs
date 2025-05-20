using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private GameObject _bulletImpactFX;
    private Rigidbody _rb => GetComponent<Rigidbody>();

    private void Update()
    {
        if (_bulletLifeTime <= 0 && gameObject.activeSelf)
        {
            _bulletLifeTime = 5;
            ReturnBulletToPool();
        }
        _bulletLifeTime -= Time.deltaTime;
    }

    private void ReturnBulletToPool()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        GetComponent<TrailRenderer>().Clear();
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contactPoint = collision.contacts[0];
            //GameObject newImpactFX = Instantiate(_bulletImpactFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            GameObject newImpactFX = ObjectPool.Instance.GetObject(_bulletImpactFX);
            newImpactFX.transform.position = contactPoint.point;
            ObjectPool.Instance.ReturnObject(newImpactFX, 1f);
        }
    }
}
