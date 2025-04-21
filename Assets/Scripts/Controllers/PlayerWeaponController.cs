using UnityEngine;

namespace Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Shoot");

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private Transform _gunPoint;
        
        private Player _player;
        private Animator _animator;
    
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _player = GetComponent<Player>();
            AssignInputEvents();
        }

        private void AssignInputEvents()
        {
            _player.PlayerControls.Character.Shoot.performed += ctx => Shoot();
        }

        private void Shoot()
        {
            //Instantiate(_bulletPrefab, _gunPoint.position, Quaternion.LookRotation(_gunPoint.forward));
            var newBullet = ObjectPool.Instance.GetBullet();
            newBullet.transform.position = _gunPoint.position;
            newBullet.transform.rotation = Quaternion.LookRotation(_gunPoint.forward);
            newBullet.GetComponent<Rigidbody>().linearVelocity = _gunPoint.forward * _bulletSpeed;
            _animator.SetTrigger(Fire);
        }
    }
}
