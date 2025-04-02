using Sirenix.OdinInspector;
using UnityEngine;

namespace Controls
{
    public class Mover : MonoBehaviour
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int ZVelocity = Animator.StringToHash("zVelocity");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Fire = Animator.StringToHash("Shoot");

        [FoldoutGroup("MovementSettings")] [SerializeField]
        private float _walkSpeed = 5f;

        [FoldoutGroup("MovementSettings")] [SerializeField]
        private float _runningSpeed = 10f;

        [FoldoutGroup("MovementSettings")] [SerializeField]
        private float _verticalVelocity = 0f;

        private Player _player;
        private float _speed;
        private Vector3 _movementDirection;
        private Vector2 _moveInput;
        private bool _isRunning;


        [FoldoutGroup("Aim Settings")] [SerializeField]
        private LayerMask _aimLayerMask;

        [FoldoutGroup("Aim Settings")] [SerializeField]
        private Transform _aim;

        private Vector3 _lookingDirection;
        private Vector2 _aimInput;

        private PlayerControls _playerControls;
        private CharacterController _characterController;
        private Animator _animator;

        private void Start()
        {
            _player = GetComponent<Player>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            _speed = _walkSpeed;
            AssignInputEvents();
        }


        private void Update()
        {
            ApplyMovement();
            AimTowardsMouse();
            GetAnimatorControllers();
        }

        

        private void GetAnimatorControllers()
        {
            float xVelocity = Vector3.Dot(_movementDirection.normalized, transform.right);
            float zVelocity = Vector3.Dot(_movementDirection.normalized, transform.forward);

            _animator.SetFloat(XVelocity, xVelocity, .1f, Time.deltaTime);
            _animator.SetFloat(ZVelocity, zVelocity, .1f, Time.deltaTime);

            bool playRunAnimation = _isRunning && _movementDirection.magnitude > 0;
            _animator.SetBool(IsRunning, playRunAnimation);
        }

        private void AimTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(_aimInput);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask))
            {
                _lookingDirection = hitInfo.point - transform.position;
                _lookingDirection.y = 0f;
                _lookingDirection.Normalize();

                transform.forward = _lookingDirection;

                _aim.position = new Vector3(hitInfo.point.x, transform.position.y + 1, hitInfo.point.z);
            }
        }

        private void ApplyMovement()
        {
            _movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            ApplyGravity();
            if (_movementDirection.magnitude > 0)
            {
                _characterController.Move(_movementDirection * (Time.deltaTime * _speed));
            }
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
            {
                _verticalVelocity -= 9.81f * Time.deltaTime;
                _movementDirection.y = _verticalVelocity;
            }
            else
            {
                _verticalVelocity = -0.5f;
            }
        }

        private void AssignInputEvents()
        {
            _playerControls = _player.PlayerControls;
            

            _playerControls.Character.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Movement.canceled += ctx => _moveInput = Vector2.zero;

            _playerControls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Aim.canceled += ctx => _aimInput = Vector2.zero;

            _playerControls.Character.Run.performed += ctx =>
            {
                _isRunning = true;
                _speed = _runningSpeed;
            };
            _playerControls.Character.Run.canceled += ctx =>
            {
                _isRunning = false;
                _speed = _walkSpeed;
            };
        }
    }
}