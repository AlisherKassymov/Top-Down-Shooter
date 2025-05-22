using System;
using Controllers;
using ScriptableObjects;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(_weaponData);
    }
}
