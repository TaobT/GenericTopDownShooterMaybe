using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class keeps track on ammo and ammo reserve
//Also has the reference to the weapon preset
[System.Serializable]
public class Weapon
{
    //Stats
    private WeaponPreset weapon;
    private bool isEmpty;

    public WeaponPreset WeaponStats { get { return weapon; } }

    //Ammo
    public int CurrentAmmo { get; private set; }
    public int CurrentAmmoReserve { get; private set; }

    public Weapon()
    {
        isEmpty = true;
    }

    public Weapon(WeaponPreset preset)
    {
        weapon = preset;
        CurrentAmmo = preset.maxAmmo;
        CurrentAmmoReserve = preset.ammoReserve;
        isEmpty = false;
    }

    public Weapon(Weapon copy)
    {
        weapon = copy.WeaponStats;
        CurrentAmmo = copy.CurrentAmmo;
        CurrentAmmoReserve = copy.CurrentAmmoReserve;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public bool IsMelee()
    {
        return WeaponStats.weaponSlot == WeaponPreset.WeaponSlot.Melee;
    }

    //Ammo methods
    public void RemoveAmmo(int ammoToRemove = 1)
    {
        if(CurrentAmmo - ammoToRemove >= 0)
        {
            CurrentAmmo -= ammoToRemove;
        }
    }

    public bool WeaponHasAmmo()
    {
        return CurrentAmmo > 0;
    }

    public bool WeaponHasFullAmmo()
    {
        return CurrentAmmo == weapon.maxAmmo;
    }

    public void SetAmmo(int ammoToReload)
    {
        CurrentAmmo = ammoToReload;
        CurrentAmmo = Mathf.Clamp(CurrentAmmo, 0, weapon.maxAmmo);
    }

    public int RemoveAmmoFromReserve(int ammoToTake)
    {
        if(CurrentAmmoReserve - ammoToTake < 0)
        {
            ammoToTake = CurrentAmmoReserve;
            CurrentAmmoReserve = 0;
        }
        else
        {
            CurrentAmmoReserve -= ammoToTake;
        }

        return ammoToTake;
    }
    
    public bool IsAmmoInReserve()
    {
        return CurrentAmmoReserve > 0;
    }
}
