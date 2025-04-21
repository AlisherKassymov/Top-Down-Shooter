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
            var newBullet = Instantiate(_bulletPrefab, _gunPoint.position, Quaternion.LookRotation(_gunPoint.forward));
            newBullet.GetComponent<Rigidbody>().linearVelocity = _gunPoint.forward * _bulletSpeed;
            Destroy(newBullet, 10);
            _animator.SetTrigger(Fire);
        }
    }
}
