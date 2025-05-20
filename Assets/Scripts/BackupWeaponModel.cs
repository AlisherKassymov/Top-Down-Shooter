using System;
using UnityEngine;

public class BackupWeaponModel : MonoBehaviour
{
    [SerializeField] private HangType _hangType;
    public WeaponType WeaponType;


    public void Activate(bool activated)
    {
        gameObject.SetActive(activated);
    }
    public bool GetHangType(HangType hangType)
    {
        return _hangType == hangType;
    }
}

public enum HangType
{
    LowBackHang,
    BackHang,
    SideHang
}
