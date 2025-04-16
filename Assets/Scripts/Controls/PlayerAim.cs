using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controls
{
    public class PlayerAim : MonoBehaviour
    {
        [FoldoutGroup("Aim Settings")] [SerializeField]
        private LayerMask _aimLayerMask;

        [FoldoutGroup("Aim Settings")] [SerializeField]
        private Transform _aim;

        private Vector3 _lookingDirection;
        private Player _player;
        private PlayerControls _playerControls;
        private Vector3 _aimInput;

        private void Start()
        {
            _player = GetComponent<Player>();
            AssignInputEvents();
        }

        private void Update()
        {
            _aim.position = new Vector3(GetMousePosition().x, transform.position.y, GetMousePosition().z);
        }

        public Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(_aimInput);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
            {
                return hitInfo.point;
            }

            return Vector3.zero;
        }

        private void AssignInputEvents()
        {
            _playerControls = _player.PlayerControls;

            _playerControls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Aim.canceled += ctx => _aimInput = Vector2.zero;
        }
    }
}