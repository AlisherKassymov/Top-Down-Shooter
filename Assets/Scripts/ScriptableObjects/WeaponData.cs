using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName;
        [FoldoutGroup("Normal shot")] public ShootingMode ShootingMode;
        [FoldoutGroup("Normal shot")] public int BulletsPerShot = 1;
        [FoldoutGroup("Normal shot")] public float FireRate;
        
        [FoldoutGroup("Magazine Details")]  public int BulletsInMagazine;
        [FoldoutGroup("Magazine Details")] public int MagazineCapacity;
        [FoldoutGroup("Magazine Details")]  public int TotalReservedAmmo;

        [FoldoutGroup("Weapon Settings")] public WeaponType WeaponType;

        [Range(1, 2)] [FoldoutGroup("Weapon Settings")]
        public float ReloadSpeed = 1;

        [Range(1, 2)] [FoldoutGroup("Weapon Settings")]
        public float EquipSpeed = 1;

        [Range(3, 8)] [FoldoutGroup("Weapon Settings")]
        public float CameraDistance = 6;

        [FoldoutGroup("Burst Fire")] public bool IsBurstAvailable;
        [FoldoutGroup("Burst Fire")] public bool IsBurstActive;
        [FoldoutGroup("Burst Fire")] public float BurstFireDelay = 0.1f;
        [FoldoutGroup("Burst Fire")] public int BurstModeBulletsPerShot;
        [FoldoutGroup("Burst Fire")] public float BurstModeFireRate;

        [FoldoutGroup("Spread settings")] public float BaseSpread = 1;
        [FoldoutGroup("Spread settings")] public float MaxSpread = 3;
        [FoldoutGroup("Spread settings")] public float spreadIncreaseRate = 0.15f;
    }
}