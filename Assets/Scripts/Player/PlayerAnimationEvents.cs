using System;
using Controllers;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals _playerWeaponVisuals;
    private PlayerWeaponController _playerWeaponController;

    private void Start()
    {
        _playerWeaponVisuals = GetComponentInParent<PlayerWeaponVisuals>();
        _playerWeaponController = GetComponentInParent<PlayerWeaponController>();
    }

    public void CompleteReloadAnimation()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        _playerWeaponController.CurrenWeapon().ReloadMagazine();
        _playerWeaponController.SetWeaponReady(true);
    }

    public void ReturnRig()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        _playerWeaponVisuals.MaximizeLeftHandIKWeight();
    }
    public void CompleteWeaponGrab()
    {
        _playerWeaponController.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => _playerWeaponVisuals.SwitchOnCurrentWeaponModel();
}
