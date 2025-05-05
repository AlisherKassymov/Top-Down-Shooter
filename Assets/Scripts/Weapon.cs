using System;
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

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (BulletsInMagazine > 0)
        {
            BulletsInMagazine--;
            return true;
        }

        return false;
    }

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
}

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}
