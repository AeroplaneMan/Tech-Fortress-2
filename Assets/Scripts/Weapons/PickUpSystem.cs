using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    public ProjectileGun gunScript;
    public WeaponSway sway;

    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;
    public Camera weaponCam;
    public TextMeshPro ammunitionDisplay;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        slotFull = false;

        if (!equipped) { 
            gunScript.enabled = false;
            sway.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            ammunitionDisplay.enabled = false;
            weaponCam.enabled = false;
        }
        if (equipped)
        {
            gunScript.enabled = true;
            sway.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            ammunitionDisplay.enabled = true;
            weaponCam.enabled = true;
        }
    }
    private void Update() { 
        Vector3 distanceToPlayer = player.position - transform.position;
        if (!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        if (equipped && Input.GetKeyDown(KeyCode.F)) Drop();
    }
    private void PickUp() { 
        equipped = true;
        slotFull = true;

        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        rb.isKinematic = true;
        coll.isTrigger = true;

        gunScript.enabled = true;
        sway.enabled = true;
        ammunitionDisplay.enabled = true;
        weaponCam.enabled = true;
    }
    private void Drop() {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;
        
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random,random,random) * 10);
        
        gunScript.enabled = false;
        sway.enabled = false;
        ammunitionDisplay.enabled = false;
        weaponCam.enabled = false;
    }
}
