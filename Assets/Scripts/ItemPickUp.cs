using System;
using Controllers;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickUpWeapon(_weapon);
    }
}
