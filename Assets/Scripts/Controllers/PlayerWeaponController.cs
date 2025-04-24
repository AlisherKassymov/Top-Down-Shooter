using System;
using UnityEngine;

namespace Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Shoot");

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private Transform _gunPoint;

        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private Transform _aim;
        
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
            newBullet.GetComponent<Rigidbody>().linearVelocity = GetBulletDirection() * _bulletSpeed;
            _animator.SetTrigger(Fire);
        }

        private Vector3 GetBulletDirection()
        {
            Vector3 direction = (_aim.position - _gunPoint.position).normalized;
            if (_player.PlayerAim.CanAim() == false && _player.PlayerAim.ReturnTarget() == null)
            {
                direction.y = 0;
            }
            _weaponHolder.LookAt(_aim);
            _gunPoint.LookAt(_aim);
            return direction;
        }
    }
}
