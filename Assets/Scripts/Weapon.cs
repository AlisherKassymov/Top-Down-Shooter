using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Weapon
{
    public WeaponType WeaponType;
    [FormerlySerializedAs("Ammo")] public int BulletsInMagazine;
    public int MagazineCapacity;
    [FormerlySerializedAs("MaxAmmo")] public int TotalReservedAmmo;

    [Range(1, 2)]
    public float ReloadSpeed = 1;
    [Range(1,2)]
    public float EquipSpeed = 1;

    [Space]
    public float FireRate = 1;

    private float _lastShootTime;
    public bool CanShoot()
    {
        if (HaveEnoughBullets() && IsReadyToShoot())
        {
            BulletsInMagazine--;
            return true;
        }
        return false;
    }

    private bool IsReadyToShoot()
    {
        if (Time.time > _lastShootTime + 1 / FireRate)
        {
            _lastShootTime = Time.time;
            return true;
        }
        return false;
    }
    
    #region Reload Methods

    private bool HaveEnoughBullets() => BulletsInMagazine > 0;

    public bool CanReload()
    {
        if (BulletsInMagazine == MagazineCapacity)
        {
            return false;
        }
        if (TotalReservedAmmo > 0)
        {
            return true;
        }

        return false;
    }

    public void ReloadMagazine()
    {
        int bulletsToReload = MagazineCapacity - BulletsInMagazine;
        if (bulletsToReload > TotalReservedAmmo)
        {
            bulletsToReload = TotalReservedAmmo;
        }

        TotalReservedAmmo -= bulletsToReload;
        BulletsInMagazine += bulletsToReload;

        if (TotalReservedAmmo < 0)
        {
            TotalReservedAmmo = 0;
        }
    }

    #endregion
}

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}
