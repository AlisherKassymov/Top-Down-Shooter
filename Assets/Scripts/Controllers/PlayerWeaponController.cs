using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Shoot");
        private const float REFERENCE_BULLET_SPEED = 20;

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private Transform _weaponHolder;
        
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
            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
            newBullet.transform.position = _gunPoint.position;
            newBullet.transform.rotation = Quaternion.LookRotation(_gunPoint.forward);
            rbNewBullet.mass = REFERENCE_BULLET_SPEED / _bulletSpeed;
            rbNewBullet.linearVelocity = GetBulletDirection() * _bulletSpeed;
            _animator.SetTrigger(Fire);
        }

        public Vector3 GetBulletDirection()
        {
            Transform aim = _player.PlayerAim.GetAim();
            Vector3 direction = (aim.position - _gunPoint.position).normalized;
            if (_player.PlayerAim.CanAim() == false && _player.PlayerAim.ReturnTarget() == null)
            {
                direction.y = 0;
            }
            /*_weaponHolder.LookAt(_aim);
            _gunPoint.LookAt(_aim); TODO: Find a better place; Refactor*/ 
            return direction;
        }
        
        public Transform GetGunPoint() => _gunPoint;
    }
}
