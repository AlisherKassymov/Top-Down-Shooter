
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
   public WeaponType WeaponType;
   public GrabType GrabType;
   public HoldType HoldType;
   
   public Transform GunPoint;
   public Transform HoldPoint;
}

public enum HoldType
{
   CommonHold = 1,
   LowHold,
   HighHold
}
public enum GrabType
{
   SideGrab, 
   BackGrab
}
