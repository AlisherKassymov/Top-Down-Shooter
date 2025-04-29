using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Shoot");
        private const float REFERENCE_BULLET_SPEED = 20;

        [FoldoutGroup("Bullet Settings")]
        [SerializeField] private GameObject _bulletPrefab;
        [FoldoutGroup("Bullet Settings")]
        [SerializeField] private float _bulletSpeed;
        [FoldoutGroup("Bullet Settings")]
        [SerializeField] private Transform _gunPoint;

        [FoldoutGroup("Inventory")] [SerializeField]
        private List<Weapon> _weaponSlots;

        [FoldoutGroup("Inventory")]
        private int _maxSlots = 2;
        
        [SerializeField] private Transform _weaponHolder;
        
        [SerializeField] private Weapon _currentWeapon;
        
        private Player _player;
        private Animator _animator;
    
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            _player = GetComponent<Player>();
            _currentWeapon.BulletsInMagazine = _currentWeapon.TotalReservedAmmo;
            AssignInputEvents();
        }
        

        private void Shoot()
        {
            if (_currentWeapon.CanShoot() == false)
            {
                return;
            }
            //Instantiate(_bulletPrefab, _gunPoint.position, Quaternion.LookRotation(_gunPoint.forward));
            var newBullet = ObjectPool.Instance.GetBullet();
            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
            newBullet.transform.position = _gunPoint.position;
            newBullet.transform.rotation = Quaternion.LookRotation(_gunPoint.forward);
            rbNewBullet.mass = REFERENCE_BULLET_SPEED / _bulletSpeed;
            rbNewBullet.linearVelocity = GetBulletDirection() * _bulletSpeed;
            _animator.SetTrigger(Fire);
        }

        private void EquipWeapon(int index)
        {
            _currentWeapon = _weaponSlots[index];
        }

        private void DropWeapon()
        {
            if (_weaponSlots.Count <= 1)
            {
                return;
            }
            _weaponSlots.Remove(_currentWeapon);
            _currentWeapon = _weaponSlots[0];
        }

        public void PickUpWeapon(Weapon newWeapon)
        {
            if (_weaponSlots.Count >= _maxSlots)
            {
                Debug.Log("No slots available");
                return;
            }

            _weaponSlots.Add(newWeapon);
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
        
        private void AssignInputEvents()
        {
            PlayerControls playerControls = _player.PlayerControls;
            
            playerControls.Character.Shoot.performed += ctx => Shoot();
            
            playerControls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
            playerControls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);
            playerControls.Character.DropCurrentWeapon.performed += ctx => DropWeapon();
            playerControls.Character.Reload.performed += ctx =>
            {
                if (_currentWeapon.CanReload())
                {
                    _player.PlayerWeaponVisuals.PlayReloadAnimation();
                }
            };
        }

        public Weapon CurrenWeapon() => _currentWeapon;
        public Transform GetGunPoint() => _gunPoint;
    }
}
