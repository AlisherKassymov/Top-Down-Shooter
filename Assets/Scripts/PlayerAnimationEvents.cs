using System;
using Controllers;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals _playerWeaponVisuals;

    private void Start()
    {
        _playerWeaponVisuals = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void CompleteReloadAnimation()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        //Refill ammo
    }

    public void ReturnRig()
    {
        _playerWeaponVisuals.MaximizeRigWeight();
        _playerWeaponVisuals.MaximizeLeftHandIKWeight();
    }
    public void CompleteWeaponGrab()
    {
        _playerWeaponVisuals.SetBusyGrabbingWeaponTo(false);
    }
}
