using System;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class Weapon
{
    public WeaponType WeaponType;

    [FoldoutGroup("Shooting settings")] public ShootingMode ShootingMode;
    [FoldoutGroup("Shooting settings")] private float FireRate = 1;
    [FoldoutGroup("Shooting settings")] private float _defaultFireRate;
    [FoldoutGroup("Shooting settings")] private float _lastShootTime;
    [FoldoutGroup("Shooting settings")] public int BulletsPerShot { get; private set; }
    
    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("Ammo")] public int BulletsInMagazine;
    [FoldoutGroup("Magazine Details")] public int MagazineCapacity;
    [FoldoutGroup("Magazine Details")] [FormerlySerializedAs("MaxAmmo")] public int TotalReservedAmmo;

    [FoldoutGroup("Speed Settings")] public float ReloadSpeed{ get; private set; }
    [FoldoutGroup("Speed Settings")] public float EquipSpeed { get; private set; }
    
    [FoldoutGroup("Burst Fire")] private bool _isBurstAvailable;
    [FoldoutGroup("Burst Fire")] private int _burstModeBulletsPerShot;
    [FoldoutGroup("Burst Fire")] private float _burstModeFireRate;
    [FoldoutGroup("Burst Fire")] public float BurstFireDelay { get; private set; }
    [FoldoutGroup("Burst Fire")] public bool IsBurstActive;

    [FoldoutGroup("Spread settings")] private float _baseSpread = 1;
    [FoldoutGroup("Spread settings")] private float _currentSpread = 1;
    [FoldoutGroup("Spread settings")] private float _maxSpread = 3;
    [FoldoutGroup("Spread settings")] private float _spreadIncreaseRate = 0.15f;
    
    [FoldoutGroup("CameraSettings")] [Range(3,8)] public float CameraDistance {get; private set; }
    private float _lastSpreadUpdateTime;
    private float _spreadCooldown = 1;


    public Weapon(WeaponData weaponData)
    {
        FireRate = weaponData.FireRate;
        WeaponType = weaponData.WeaponType;
        
        BulletsPerShot = weaponData.BulletsPerShot;
        ShootingMode = weaponData.ShootingMode;
        _defaultFireRate = FireRate;

        BulletsPerShot = weaponData.BulletsPerShot;
        BulletsInMagazine = weaponData.BulletsInMagazine;
        MagazineCapacity = weaponData.MagazineCapacity;
        TotalReservedAmmo = weaponData.TotalReservedAmmo;
        
        _baseSpread = weaponData.BaseSpread;
        _maxSpread = weaponData.MaxSpread;
        _spreadIncreaseRate = weaponData.spreadIncreaseRate;
        
        ReloadSpeed = weaponData.ReloadSpeed;
        EquipSpeed = weaponData.EquipSpeed;
        CameraDistance = weaponData.CameraDistance;
        
        _isBurstAvailable = weaponData.IsBurstAvailable;
        IsBurstActive = weaponData.IsBurstActive;
        BurstFireDelay = weaponData.BurstFireDelay;
        _burstModeBulletsPerShot = weaponData.BurstModeBulletsPerShot;
        _burstModeFireRate = weaponData.BurstModeFireRate;
        
    }

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
        if (_isBurstAvailable == false)
        {
            return;
        }
        IsBurstActive = !IsBurstActive;
        if (IsBurstActive)
        {
            BulletsPerShot = _burstModeBulletsPerShot;
            FireRate = _burstModeFireRate;
        }
        else
        {
            BulletsPerShot = 1;
            FireRate = _defaultFireRate;
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
        float randomizedValue = Random.Range(-_currentSpread, _currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);
        return spreadRotation * originalDirection;
    }

    private void IncreaseSpread()
    {
        _currentSpread = Mathf.Clamp(_currentSpread + _spreadIncreaseRate, _baseSpread, _maxSpread);
    }

    private void UpdateSpread()
    {
        if (Time.time > _lastSpreadUpdateTime + _spreadCooldown)
        {
            _currentSpread = _baseSpread;
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