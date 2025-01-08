using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType Gun;
    [SerializeField] private Transform GunParent;
    [SerializeField] private List<WeaponScriptableObject> Weapons;
    //[SerializeField] private PlayerIK InverseKinematics;

    [Space]
    [Header("Runtime Filled")]
    public WeaponScriptableObject activeWeapon;

    private void Start()
    {
        WeaponScriptableObject weapon = Weapons.Find(weapon => weapon.weaponType == Gun);

        if (weapon == null) {
            Debug.LogError($"No WeaponScriptableObject found for GunType: {weapon}");
            return;
        }

        activeWeapon = weapon;
        weapon.Spawn(GunParent, this);
    }
}
