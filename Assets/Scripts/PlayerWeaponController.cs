using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private static readonly int Fire = Animator.StringToHash("Shoot");
    private Player _player;
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponent<Player>();
        _player.PlayerControls.Character.Shoot.performed += ctx => Shoot();
    }

    private void Shoot()
    {
        _animator.SetTrigger(Fire);
    }
}
