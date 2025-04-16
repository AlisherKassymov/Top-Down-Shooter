using Sirenix.OdinInspector;
using UnityEngine;

namespace Controls
{
    public class Mover : MonoBehaviour
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int ZVelocity = Animator.StringToHash("zVelocity");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

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
            ApplyRotation();
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

        private void ApplyRotation()
        {
            var lookingDirection = _player.PlayerAim.GetMousePosition() - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;
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