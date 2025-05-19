using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class Weapon
{
    public WeaponType WeaponType;

    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("Ammo")]
    public int BulletsInMagazine;
    [FoldoutGroup("Magazine Details")] public int MagazineCapacity;
    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("MaxAmmo")]
    public int TotalReservedAmmo;

    [Space] [Range(1, 2)] [FoldoutGroup("Speed Settings")] public float ReloadSpeed = 1;
    [Range(1, 2)] [FoldoutGroup("Speed Settings")] public float EquipSpeed = 1;
    
    [FoldoutGroup("Burst Fire")] public bool IsBurstAvailable;
    [FoldoutGroup("Burst Fire")] public bool IsBurstActive;
    [FoldoutGroup("Burst Fire")] public float BurstFireDelay;
    [FoldoutGroup("Burst Fire")] public int BurstModeBulletsPerShot;
    [FoldoutGroup("Burst Fire")] public float BurstModeFireRate;

    [FoldoutGroup("Shooting settings")] public float FireRate = 1;
    [FoldoutGroup("Shooting settings")] public float DefaultFireRate;
    [FoldoutGroup("Shooting settings")] public ShootingMode ShootingMode;
    [FoldoutGroup("Shooting settings")] private float _lastShootTime;
    [FoldoutGroup("Shooting settings")] public int BulletsPerShot;
    
    [FoldoutGroup("Spread settings")] public float BaseSpread = 1;
    [FoldoutGroup("Spread settings")] public float CurrentSpread = 1;
    [FoldoutGroup("Spread settings")] public float MaxSpread = 3;
    [FoldoutGroup("Spread settings")] public float spreadIncreaseRate = 0.15f;
    private float _lastSpreadUpdateTime;
    private float _spreadCooldown = 1;


    #region Burst methods

    public bool IsBurstActivated()
    {
        if (WeaponType == WeaponType.Shotgun)
        {
            BurstFireDelay = 0;
            return true;
        }

        return IsBurstActive;
    }

    public void ToggleBurstMode()
    {
        if (IsBurstAvailable == false)
        {
            return;
        }
        IsBurstActive = !IsBurstActive;
        if (IsBurstActive)
        {
            BulletsPerShot = BurstModeBulletsPerShot;
            FireRate = BurstModeFireRate;
        }
        else
        {
            BulletsPerShot = 1;
            FireRate = DefaultFireRate;
        }
    }

    #endregion
    public bool CanShoot()
    {
        if (HaveEnoughBullets() && IsReadyToShoot())
        {
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

    #region SpreadMethods

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();
        float randomizedValue = Random.Range(-CurrentSpread, CurrentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);
        return spreadRotation * originalDirection;
    }

    private void IncreaseSpread()
    {
        CurrentSpread = Mathf.Clamp(CurrentSpread + spreadIncreaseRate, BaseSpread, MaxSpread);
    }

    private void UpdateSpread()
    {
        if (Time.time > _lastSpreadUpdateTime + _spreadCooldown)
        {
            CurrentSpread = BaseSpread;
        }
        else
        {
            IncreaseSpread();
        }

        _lastSpreadUpdateTime = Time.time;
    }

    #endregion

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