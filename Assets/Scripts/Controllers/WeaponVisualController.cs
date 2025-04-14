using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace Controllers
{
    public class WeaponVisualController : MonoBehaviour
    {
        private static readonly int Reload = Animator.StringToHash("Reload");
        private static readonly int WeaponGrabType = Animator.StringToHash("WeaponGrabType");
        private static readonly int WeaponGrab = Animator.StringToHash("WeaponGrab");
        private static readonly int BusyGrabbingWeapon = Animator.StringToHash("BusyGrabbingWeapon");
        [SerializeField] private Transform[] _gunTransforms;

        [SerializeField] private Transform _pistol;
        [SerializeField] private Transform _revolver;
        [SerializeField] private Transform _rifle;
        [SerializeField] private Transform _shotgun;
        [SerializeField] private Transform _sniperRifle;

        [Header("Left Hand IK")]
        [SerializeField] private TwoBoneIKConstraint _leftHandIK;
        [SerializeField]private Transform _leftHandIKTarget;
        [SerializeField] private float _leftHandIKIncreaseStep;
        private bool _leftHandIKActive;
        

        [Header("Rig")] [SerializeField] private float _rigIncreaseStep;
        private bool _rigShouldBeIncreased;
        private Rig _rig;

        private Transform _currentGun;
        private Animator _animator;
        private bool _busyGrabbingWeapon;


        private void Start()
        {
            SwitchOnWeapon(_pistol);
            _animator = GetComponentInChildren<Animator>();
            _rig = GetComponentInChildren<Rig>();
        }

        private void Update()
        {
            CheckWeaponSwitch();
            if (Input.GetKeyDown(KeyCode.R) && _busyGrabbingWeapon == false)
            {
                _animator.SetTrigger(Reload);
                PauseRig();
            }
            
            UpdateRigWeight();

            UpdateLeftHandIKWeight();
        }

        private void UpdateLeftHandIKWeight()
        {
            if (_leftHandIKActive)
            {
                _leftHandIK.weight += _leftHandIKIncreaseStep * Time.deltaTime;
                if (_leftHandIK.weight >= 1)
                {
                    _leftHandIKActive = false;
                    
                }
            }
        }

        private void UpdateRigWeight()
        {
            if (_rigShouldBeIncreased)
            {
                _rig.weight += _rigIncreaseStep * Time.deltaTime;
                if (_rig.weight >= 1)
                {
                    _rigShouldBeIncreased = false;
                }
            }
        }

        private void PauseRig()
        {
            _rig.weight = .15f;
        }

        private void PlayWeaponGrabAnimation(GrabType grabType)
        {
            _leftHandIK.weight = 0;
            PauseRig();
            _animator.SetFloat(WeaponGrabType, (float)grabType);
            _animator.SetTrigger(WeaponGrab);
            
            SetBusyGrabbingWeaponTo(true);
        }

        public void SetBusyGrabbingWeaponTo(bool isBusyGrabbingWeapon)
        {
            _busyGrabbingWeapon = isBusyGrabbingWeapon;
            _animator.SetBool(BusyGrabbingWeapon, _busyGrabbingWeapon);
        }

        public void ReturnRigWeightToOne() => _rigShouldBeIncreased = true;
        public void ReturnLeftHandIK() => _leftHandIKActive = true;

        private void SwitchOnWeapon(Transform weapon)
        {
            SwitchOffWeapons();
            weapon.gameObject.SetActive(true);
            _currentGun = weapon;
            AttachLeftHand();
        }

        private void SwitchOffWeapons()
        {
            foreach (var t in _gunTransforms)
            {
                t.gameObject.SetActive(false);
            }
        }

        private void AttachLeftHand()
        {
            Transform targetTransform = _currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
            _leftHandIKTarget.localPosition = targetTransform.localPosition;
            _leftHandIKTarget.localRotation = targetTransform.localRotation;
        }

        private void SwitchAnimationLayer(int index)
        {
            for (int i = 1; i < _animator.layerCount; i++)
            {
                _animator.SetLayerWeight(i,0);
            }
            _animator.SetLayerWeight(index,1);
        }

        private void CheckWeaponSwitch()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchOnWeapon(_pistol);
                SwitchAnimationLayer(1);
                PlayWeaponGrabAnimation(GrabType.SideGrab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchOnWeapon(_revolver);
                SwitchAnimationLayer(1);
                PlayWeaponGrabAnimation(GrabType.SideGrab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchOnWeapon(_rifle);
                SwitchAnimationLayer(1);
                PlayWeaponGrabAnimation(GrabType.BackGrab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchOnWeapon(_shotgun);
                SwitchAnimationLayer(2);
                PlayWeaponGrabAnimation(GrabType.BackGrab);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchOnWeapon(_sniperRifle);
                SwitchAnimationLayer(3);
                PlayWeaponGrabAnimation(GrabType.BackGrab);
            }
        }

        public enum GrabType
        {
            SideGrab, 
            BackGrab
        }
    }
}