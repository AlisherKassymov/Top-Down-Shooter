using System;

[Serializable]
public class Weapon
{
    public WeaponType WeaponType;
    public int Ammo;
    public int MaxAmmo;
}

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}
