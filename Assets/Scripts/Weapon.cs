using System;
using Sirenix.OdinInspector;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Weapon
{
    public WeaponType WeaponType;

    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("Ammo")]
    public int BulletsInMagazine;

    [FoldoutGroup("Magazine Details")] public int MagazineCapacity;

    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("MaxAmmo")]
    public int TotalReservedAmmo;

    [Space] [Range(1, 2)] [FoldoutGroup("Speed Settings")]
    public float ReloadSpeed = 1;

    [Range(1, 2)] [FoldoutGroup("Speed Settings")]
    public float EquipSpeed = 1;

    [Space] [FoldoutGroup("Shooting settings")]
    private float _lastShootTime;

    [FoldoutGroup("Shooting settings")] public float FireRate = 1;
    [FoldoutGroup("Shooting settings")] public ShootingMode ShootingMode;

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

public enum ShootingMode
{
    Single,
    Auto
}