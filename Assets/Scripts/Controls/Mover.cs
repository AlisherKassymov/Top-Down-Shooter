using Sirenix.OdinInspector;
using UnityEngine;

namespace Controls
{
    public class Mover : MonoBehaviour
    {
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");
        private static readonly int ZVelocity = Animator.StringToHash("zVelocity");
        
        [FoldoutGroup("MovementSettings")][SerializeField] private float _movementSpeed = 5f;
        [FoldoutGroup("MovementSettings")][SerializeField] private float _verticalVelocity = 0f;
        private Vector3 _movementDirection;
        private Vector2 _moveInput; 

        [FoldoutGroup("Aim Settings")] [SerializeField] private LayerMask _aimLayerMask;
        [FoldoutGroup("Aim Settings")] [SerializeField] private Transform _aim;
        private Vector3 _lookingDirection;
        private Vector2 _aimInput; 
        
        private PlayerControls _playerControls;
        private CharacterController _characterController;
        private Animator _animator;
        

        private void Awake()
        {
            _playerControls = new PlayerControls();
            
            _playerControls.Character.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Movement.canceled += ctx => _moveInput = Vector2.zero;
            
            _playerControls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
            _playerControls.Character.Aim.canceled += ctx => _aimInput = Vector2.zero;
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
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
            _animator.SetFloat(ZVelocity, zVelocity, .1f , Time.deltaTime);
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

                _aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
            }
        }

        private void ApplyMovement()
        {
            _movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            ApplyGravity();
            if (_movementDirection.magnitude > 0)
            {
                _characterController.Move(_movementDirection * (Time.deltaTime * _movementSpeed));
            }
        }

        private void ApplyGravity()
        {
            if (!_characterController.isGrounded)
            {
                _verticalVelocity -= 9.81f *Time.deltaTime;
                _movementDirection.y = _verticalVelocity;
            }
            else
            {
                _verticalVelocity = -0.5f;
            }
        }
    }
}
