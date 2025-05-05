using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace Controllers
{
    public class PlayerWeaponVisuals : MonoBehaviour
    {
        private static readonly int Reload = Animator.StringToHash("Reload");
        private static readonly int WeaponGrabType = Animator.StringToHash("WeaponGrabType");
        private static readonly int WeaponGrab = Animator.StringToHash("WeaponGrab");
        private static readonly int BusyGrabbingWeapon = Animator.StringToHash("BusyGrabbingWeapon");
        private static readonly int EquipSpeed = Animator.StringToHash("EquipSpeed");
        private static readonly int ReloadSpeed = Animator.StringToHash("ReloadSpeed");


        [SerializeField] private WeaponModel[] _weaponModels;
        [SerializeField] private BackupWeaponModel[] _backupWeapons;

        [Header("Left Hand IK")] [SerializeField]
        private TwoBoneIKConstraint _leftHandIK;

        [SerializeField] private Transform _leftHandIKTarget;

        [FormerlySerializedAs("_leftHandIKIncreaseStep")] [SerializeField]
        private float _leftHandIKWeightIncreaseRate;

        private bool _shouldIncreaseLeftHandIKWeight;


        [FormerlySerializedAs("_rigIncreaseStep")] [Header("Rig")] [SerializeField]
        private float _rigWeightIncreaseRate;

        private bool _shouldIncreaseRigWeight;
        private Rig _rig;

        private Animator _animator;
        private bool _busyGrabbingWeapon;
        private Player _player;


        private void Start()
        {
            _player = GetComponent<Player>();
            _animator = GetComponentInChildren<Animator>();
            _rig = GetComponentInChildren<Rig>();

            _weaponModels = GetComponentsInChildren<WeaponModel>(true);
            _backupWeapons = GetComponentsInChildren<BackupWeaponModel>(true);
        }

        private void Update()
        {
            UpdateRigWeight();
            UpdateLeftHandIKWeight();
        }
        

        public WeaponModel ReturnCurrenWeaponModel()
        {
            WeaponModel weaponModel = null;
            WeaponType weaponType = _player.PlayerWeaponController.CurrenWeapon().WeaponType;
            for (int i = 0; i < _weaponModels.Length; i++)
            {
                if (_weaponModels[i].WeaponType == weaponType)
                {
                    weaponModel = _weaponModels[i];
                }
            }

            return weaponModel;
        }

        public void PlayReloadAnimation()
        {
            float reloadSpeed = _player.PlayerWeaponController.CurrenWeapon().ReloadSpeed;
            if (_busyGrabbingWeapon)
            {
                return;
            }

            _animator.SetFloat(ReloadSpeed, reloadSpeed);
            _animator.SetTrigger(Reload);
            ReduceRigWeight();
        }

        public void PlayWeaponEquipAnimation()
        {
            GrabType grabType = ReturnCurrenWeaponModel().GrabType;

            float equipmentSpeed = _player.PlayerWeaponController.CurrenWeapon().EquipSpeed;
            
            _leftHandIK.weight = 0;
            ReduceRigWeight();
            _animator.SetTrigger(WeaponGrab);
            _animator.SetFloat(WeaponGrabType, (float) grabType);
            _animator.SetFloat(EquipSpeed, equipmentSpeed);
            SetBusyGrabbingWeaponTo(true);
        }

        public void SetBusyGrabbingWeaponTo(bool isBusyGrabbingWeapon)
        {
            _busyGrabbingWeapon = isBusyGrabbingWeapon;
            _animator.SetBool(BusyGrabbingWeapon, _busyGrabbingWeapon);
        }

        public void SwitchOnCurrentWeaponModel()
        {
            int animationIndex = (int) ReturnCurrenWeaponModel().HoldType;

            SwitchOffWeaponModels();
            
            SwitchOffBackUpWeaponModels();

            if (_player.PlayerWeaponController.HasOnlyOneWeapon() == false)
            {
                SwitchOnBackUpWeaponModel();
            }
            SwitchAnimationLayer(animationIndex);
            ReturnCurrenWeaponModel().gameObject.SetActive(true);
            AttachLeftHand();
        }

        public void SwitchOffWeaponModels()
        {
            foreach (var t in _weaponModels)
            {
                t.gameObject.SetActive(false);
            }
        }

        private void SwitchOffBackUpWeaponModels()
        {
            foreach (var weapon in _backupWeapons)
            {
                weapon.gameObject.SetActive(false);
            }
        }

        public void SwitchOnBackUpWeaponModel()
        {
            var weaponType = _player.PlayerWeaponController.BackupWeapon().WeaponType;
            foreach (var backupWeapon in _backupWeapons)
            {
                if (backupWeapon.WeaponType == weaponType)
                {
                    backupWeapon.gameObject.SetActive(true);
                }
            }
        }

        #region AnimationRiggingMethods

        private void UpdateLeftHandIKWeight()
        {
            if (_shouldIncreaseLeftHandIKWeight)
            {
                _leftHandIK.weight += _leftHandIKWeightIncreaseRate * Time.deltaTime;
                if (_leftHandIK.weight >= 1)
                {
                    _shouldIncreaseLeftHandIKWeight = false;
                }
            }
        }

        private void UpdateRigWeight()
        {
            if (_shouldIncreaseRigWeight)
            {
                _rig.weight += _rigWeightIncreaseRate * Time.deltaTime;
                if (_rig.weight >= 1)
                {
                    _shouldIncreaseRigWeight = false;
                }
            }
        }

        private void ReduceRigWeight()
        {
            _rig.weight = .15f;
        }

        private void AttachLeftHand()
        {
            Transform targetTransform = ReturnCurrenWeaponModel().HoldPoint;
            _leftHandIKTarget.localPosition = targetTransform.localPosition;
            _leftHandIKTarget.localRotation = targetTransform.localRotation;
        }

        public void MaximizeRigWeight() => _shouldIncreaseRigWeight = true;
        public void MaximizeLeftHandIKWeight() => _shouldIncreaseLeftHandIKWeight = true;

        private void SwitchAnimationLayer(int index)
        {
            for (int i = 1; i < _animator.layerCount; i++)
            {
                _animator.SetLayerWeight(i, 0);
            }

            _animator.SetLayerWeight(index, 1);
        }

        #endregion
    }
}