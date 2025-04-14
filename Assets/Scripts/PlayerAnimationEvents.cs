using System;
using Controllers;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private WeaponVisualController _weaponVisualController;

    private void Start()
    {
        _weaponVisualController = GetComponentInParent<WeaponVisualController>();
    }

    public void CompleteReloadAnimation()
    {
        _weaponVisualController.ReturnRigWeightToOne();
        //Refill ammo
    }

    public void ReturnRig()
    {
        _weaponVisualController.ReturnRigWeightToOne();
        _weaponVisualController.ReturnLeftHandIK();
    }
    public void CompleteWeaponGrab()
    {
        _weaponVisualController.SetBusyGrabbingWeaponTo(false);
    }
}
