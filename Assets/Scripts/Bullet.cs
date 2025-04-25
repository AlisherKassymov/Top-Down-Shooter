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
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
        _bulletLifeTime -= Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        GetComponent<TrailRenderer>().Clear();
        ObjectPool.Instance.ReturnBullet(gameObject);
    }

    private void CreateImpactFX(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contactPoint = collision.contacts[0];
            GameObject newImpactFX = Instantiate(_bulletImpactFX, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            Destroy(newImpactFX, 1f);
        }
    }
}
