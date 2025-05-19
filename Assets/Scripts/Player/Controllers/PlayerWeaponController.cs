using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Shoot");
        private const float REFERENCE_BULLET_SPEED = 20;// This is to prevent objects to move too fast if the speed of a bullet is too high

        [FoldoutGroup("Bullet Settings")]
        [SerializeField] private GameObject _bulletPrefab;
        [FoldoutGroup("Bullet Settings")]
        [SerializeField] private float _bulletSpeed;

        [FoldoutGroup("Inventory")] [SerializeField]
        private List<Weapon> _weaponSlots;

        [FoldoutGroup("Inventory")]
        private int _maxSlots = 2;
        
        [SerializeField] private Transform _weaponHolder;
        
        [SerializeField] private Weapon _currentWeapon;
        
        private Player _player;
        private bool _isWeaponReady;
        private bool _isShooting;
    
        private void Start()
        {
            _player = GetComponent<Player>();
            AssignInputEvents();
            Invoke("EquipStartingWeapon", 1f);
        }

        private void Update()
        {
            if (_isShooting)
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                _currentWeapon.ToggleBurstMode();
            }
        }

        IEnumerator BurstFire()
        {
            SetWeaponReady(false);
            for (int i = 1; i <= _currentWeapon.BulletsPerShot; i++)
            {
                FireSingleBullet();
                yield return new WaitForSeconds(_currentWeapon.BurstFireDelay);
                if (i >= _currentWeapon.BulletsPerShot)
                {
                    SetWeaponReady(true);
                }
            }
        }
        private void Shoot()
        {
            if (IsWeaponReady() == false)
            {
                return;
            }
            if (_currentWeapon.CanShoot() == false)
            {
                return;
            }
            _player.PlayerWeaponVisuals.PlayFireAnimation();

            if (_currentWeapon.ShootingMode == ShootingMode.Single)
            {
                _isShooting = false;
            }

            if (_currentWeapon.IsBurstActivated())
            {
                StartCoroutine(BurstFire());
                return;
            }
            
            FireSingleBullet();
        }

        private void FireSingleBullet()
        {
            _currentWeapon.BulletsInMagazine--;
            
            var newBullet = ObjectPool.Instance.GetBullet();
            Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
            
            newBullet.transform.position = GetGunPoint().position;
            newBullet.transform.rotation = Quaternion.LookRotation(GetGunPoint().forward);

            Vector3 bulletsDirection = _currentWeapon.ApplySpread(GetBulletDirection());
            
            rbNewBullet.mass = REFERENCE_BULLET_SPEED / _bulletSpeed;
            rbNewBullet.linearVelocity = bulletsDirection * _bulletSpeed;
        }

        private void EquipWeapon(int index)
        {
            SetWeaponReady(false);
            _currentWeapon = _weaponSlots[index];
            _player.PlayerWeaponVisuals.PlayWeaponEquipAnimation();
        }

        private void EquipStartingWeapon()
        {
            EquipWeapon(0);
        }

        private void DropWeapon()
        {
            if (HasOnlyOneWeapon())
            {
                return;
            }
            _weaponSlots.Remove(_currentWeapon);
            EquipWeapon(0);
        }

        public void SetWeaponReady(bool isReady)
        {
            _isWeaponReady = isReady;
        }
        public bool IsWeaponReady() => _isWeaponReady;
        public void PickUpWeapon(Weapon newWeapon)
        {
            if (_weaponSlots.Count >= _maxSlots)
            {
                Debug.Log("No slots available");
                return;
            }
            _weaponSlots.Add(newWeapon);
            _player.PlayerWeaponVisuals.SwitchOnBackUpWeaponModel();
        }
        
        public bool HasOnlyOneWeapon() => _weaponSlots.Count <= 1;
        public Vector3 GetBulletDirection()
        {
            Transform aim = _player.PlayerAim.GetAim();
            Vector3 direction = (aim.position - GetGunPoint().position).normalized;
            if (_player.PlayerAim.CanAim() == false && _player.PlayerAim.ReturnTarget() == null)
            {
                direction.y = 0;
            }
           
            return direction;
        }
        
        private void AssignInputEvents()
        {
            PlayerControls playerControls = _player.PlayerControls;
            
            playerControls.Character.Shoot.performed += ctx => _isShooting = true;
            playerControls.Character.Shoot.canceled += ctx => _isShooting = false;
            
            playerControls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
            playerControls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);
            playerControls.Character.DropCurrentWeapon.performed += ctx => DropWeapon();
            playerControls.Character.Reload.performed += ctx =>
            {
                if (_currentWeapon.CanReload() && _isWeaponReady)
                {
                    Reload();
                }
            };
        }

        private void Reload()
        {
            SetWeaponReady(false);
            _player.PlayerWeaponVisuals.PlayReloadAnimation();
        }

        public Weapon CurrenWeapon() => _currentWeapon;

        public Weapon BackupWeapon()
        {
            return _weaponSlots.FirstOrDefault(weapon => weapon != _currentWeapon);
        }
        public Transform GetGunPoint() => _player.PlayerWeaponVisuals.ReturnCurrenWeaponModel().GunPoint;
    }
}
