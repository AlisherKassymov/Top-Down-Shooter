using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Controllers
{
    public class WeaponVisualController : MonoBehaviour
    {
        private static readonly int Reload = Animator.StringToHash("Reload");
        [SerializeField] private Transform[] _gunTransforms;

        [SerializeField] private Transform _pistol;
        [SerializeField] private Transform _revolver;
        [SerializeField] private Transform _rifle;
        [SerializeField] private Transform _shotgun;
        [SerializeField] private Transform _sniperRifle;

        [Header("Left Hand IK")] [SerializeField]
        private Transform _leftHand;

        [Header("Rig")] [SerializeField] private float _rigIncreaseStep;
        private bool _rigShouldBeIncreased;
        private Rig _rig;

        private Transform _currentGun;
        private Animator _animator;


        private void Start()
        {
            SwitchOnWeapon(_pistol);
            _animator = GetComponentInChildren<Animator>();
            _rig = GetComponentInChildren<Rig>();
        }

        private void Update()
        {
            CheckWeaponSwitch();
            if (Input.GetKeyDown(KeyCode.R))
            {
                _animator.SetTrigger(Reload);
                _rig.weight = .15f;
            }
            
            if (_rigShouldBeIncreased)
            {
                _rig.weight += _rigIncreaseStep * Time.deltaTime;
                if (_rig.weight >= 1)
                {
                    _rigShouldBeIncreased = false;
                }
            }
        }

        public void ReturnRigWeightToOne() => _rigShouldBeIncreased = true;

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
            _leftHand.localPosition = targetTransform.localPosition;
            _leftHand.localRotation = targetTransform.localRotation;
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
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchOnWeapon(_revolver);
                SwitchAnimationLayer(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchOnWeapon(_rifle);
                SwitchAnimationLayer(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchOnWeapon(_shotgun);
                SwitchAnimationLayer(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchOnWeapon(_sniperRifle);
                SwitchAnimationLayer(3);
            }
        }
    }
}