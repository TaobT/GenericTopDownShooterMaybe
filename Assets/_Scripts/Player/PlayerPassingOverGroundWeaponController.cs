using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//This script checks if the player pass over a ground weapon
//If weapons controller has an empty slot acorrding to the ground weapon, weapons controller will pick up the ground weapon
public class PlayerPassingOverGroundWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask groundWeaponsLayerMask;
    [SerializeField] private PlayerWeaponsController weaponsController;

    private void Update()
    {
        CheckPassingOverGroundWeapon();
    }

    private void CheckPassingOverGroundWeapon()
    {
        Collider2D groundWeaponCollider = Physics2D.OverlapCircle(transform.position, 0.5f, groundWeaponsLayerMask);
        if(groundWeaponCollider != null)
        {
            GroundWeapon weapon = groundWeaponCollider.gameObject.GetComponent<GroundWeapon>();
            if (weaponsController.SlotIsEmpty(weapon.GetWeaponSlot()) && weapon.CanBePickedUp())
            {
                weaponsController.PickUpWeapon(weapon.OnPickUp(), false);
                weapon.OnPickedUp();
            }
        }
    }
}
