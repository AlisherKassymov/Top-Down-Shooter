using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controls
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private LayerMask _aimLayerMask;

        [FoldoutGroup("Aim Settings")] 
        [SerializeField] private Transform _aim;

        [SerializeField] private bool _isAiming;
        [SerializeField] private bool _isTargetLocked;
        
        [FormerlySerializedAs("_aim")]
        [FoldoutGroup("Camera Settings")] 
        [SerializeField] private Transform _cameraTarget;
        [FoldoutGroup("Camera Settings")] 
        [Range(0.5f, 1f)] [SerializeField] private float _minCameraDistance = 1.5f;
        [FoldoutGroup("Camera Settings")] 
        [Range(1f, 1.5f)] [SerializeField] private float _maxCameraDistance = 4f;
        [FormerlySerializedAs("_aimSensitivity")]
        [FoldoutGroup("Camera Settings")] 
        [Range(3f, 5f)] [SerializeField] private float _cameraSensitivity = 5f;

        private Player _player;
        private PlayerControls _playerControls;
        private Vector3 _aimInput;
        private RaycastHit _lastKnownMouseHit;

        private void Start()
        {
            _player = GetComponent<Player>();
            AssignInputEvents();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _isAiming = !_isAiming;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _isTargetLocked = !_isTargetLocked;
            }
            UpdateAimPosition();
            UpdateCameraPosition();
        }
        
        public Transform ReturnTarget()
        {
            Transform target = null;
            if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
            {
                target = GetMouseHitInfo().transform;
            }

            return target;
        }

        private void UpdateCameraPosition()
        {
            _cameraTarget.position = Vector3.Lerp(_cameraTarget.position, DesiredCameraPosition(), _cameraSensitivity * Time.deltaTime);
        }

        private void UpdateAimPosition()
        {
            Transform target = ReturnTarget();
            if (target != null && _isTargetLocked)
            {
                _aim.position = target.position;
                return;
            }
            _aim.position = GetMouseHitInfo().point;
            if (!_isAiming)
            {
                _aim.position = new Vector3(_aim.position.x, transform.position.y + 1, _aim.position.z);
            }
        }
        
        private Vector3 DesiredCameraPosition()
        {
            float actualMaxCameraDistance = _player.Mover.MoveInput.y < -0.5f ? _minCameraDistance : _maxCameraDistance;
            
            Vector3 desiredCameraPosition = GetMouseHitInfo().point;
            Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

            float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
            float clampedDistance = Math.Clamp(distanceToDesiredPosition, _minCameraDistance, actualMaxCameraDistance);

            desiredCameraPosition = transform.position + aimDirection * clampedDistance;
            desiredCameraPosition.y = transform.position.y + 1;
            
            return desiredCameraPosition;
        }
        public RaycastHit GetMouseHitInfo()
        {
            Ray ray = Camera.main.ScreenPointToRay(_aimInput);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
            {
                _lastKnownMouseHit = hitInfo;
                return hitInfo;
            }

            return _lastKnownMouseHit;
        }

        public bool CanAim()
        {
            if (_isAiming)
            {
                return true;
            }

            return false;
        }

        private void AssignInputEvents()
        {
            _playerControls = _player.PlayerControls;

            _playerControls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Aim.canceled += ctx => _aimInput = Vector2.zero;
        }
    }
}