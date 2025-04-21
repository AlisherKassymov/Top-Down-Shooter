using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb => GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision other)
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
