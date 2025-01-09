using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;       

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector gunSelector;
    private void Update()
    {
        if (Input.GetMouseButton(0) && gunSelector.activeWeapon != null) gunSelector.activeWeapon.Shoot();
    }
}
