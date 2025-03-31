using UnityEngine;

namespace Controllers
{
    public class WeaponVisualController : MonoBehaviour
    {
        [SerializeField] private Transform[] _gunTransforms;

        [SerializeField] private Transform _pistol;
        [SerializeField] private Transform _revolver;
        [SerializeField] private Transform _rifle;
        [SerializeField] private Transform _shotgun;
        [SerializeField] private Transform _sniperRifle;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchOnWeapon(_pistol);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchOnWeapon(_revolver);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchOnWeapon(_rifle);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchOnWeapon(_shotgun);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SwitchOnWeapon(_sniperRifle);
            }
        }

        private void SwitchOnWeapon(Transform weapon)
        {
            SwitchOffWeapons();
            weapon.gameObject.SetActive(true);
        }

        private void SwitchOffWeapons()
        {
            foreach (var t in _gunTransforms)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
}