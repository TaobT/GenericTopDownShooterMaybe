using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Weapon/New Weapon Preset",fileName = "Weapon Presets")]
public class WeaponPreset : ScriptableObject
{

    public string weaponName;
    [Range(0f, 100f)] public int damage;
    
    
    //Weapon Slot and Equip
    public enum WeaponSlot
    {
        Primary,
        Secondary,
        Melee
    }
    [Header("Weapon Type")]
    public WeaponSlot weaponSlot;
    [Header("Equiping")]
    [Range(0f, 3f)] public float equipTime;


    //Player Effects
    [Header("Player Effects")]
    public float playerSpeed;



    
    //Firing
    public enum FiringType
    {
        Automatic,
        SemiAutomatic
    }
    [Header("Firing")]
    public FiringType firingType;
    public float firingPoint;
    [Range(0f, 20f)] public float firePerSecond;

    //Ammo
    [Header("Ammo")]
    public int maxAmmo;
    public int ammoReserve;



    //Reloading
    public enum ReloadingType
    {
        Magazine,
        PerBullet
    }
    [Header("Reloading")]
    public ReloadingType reloadingType;
    public float reloadTime;
    [Tooltip("If reloading type is Per Bullet. How many bullets are reloaded?")]
    public int bulletsReloaded;


    [Header("Recoil")]
    [Range(0f, 180f)] public float minFiringAngle;
    [Range(0f, 180f)] public float maxFiringAngle;
    [Range(0f, 180f)] public float angleIncrement;
    [Range(0f, 180f)] public float angleDecrement;
    [Range(0f, 2f)] public float movementInaccuracyFactor;
    
    [Header("Ground Weapon")]
    public Sprite weaponGroundSprite;
    
    [Header("Equip SFX")]
    public AudioClip equip;

    [Header("Weapon SFX")]
    public AudioClip fire;
    public AudioClip reload;
    public AudioClip empty;

    [Header("Melee SFX")]
    public AudioClip miss;
    public AudioClip stab;

}