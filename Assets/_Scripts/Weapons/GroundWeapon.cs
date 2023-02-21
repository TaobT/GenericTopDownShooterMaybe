using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script allows to instatiate a visual representation of a weapon in the world
//And also keeps track on the weapon ammo
public class GroundWeapon : Interactable
{
    private SpriteRenderer groundWeaponSprite;
    private Rigidbody2D rb;
    private Weapon weapon;

    [SerializeField] private WeaponPreset initialWeaponPreset;
    [SerializeField] private float throwForce;
    [SerializeField] private float turningForce;
    [SerializeField] private float throwPickUpCooldown;
    private float throwPickUpCooldownEndTime;

    public override InteractionType type { get => InteractionType.PickUp;}

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundWeaponSprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(initialWeaponPreset != null)
        {
            Spawned(new Weapon(initialWeaponPreset));
        }
    }

    private void InitGroundWeapon(Weapon weapon)
    {
        this.weapon = new Weapon(weapon);
        groundWeaponSprite.sprite = weapon.WeaponStats.weaponGroundSprite;
    }

    public void Throwed(Weapon weapon)
    {
        InitGroundWeapon(weapon);
        rb.AddForce(transform.right * throwForce, ForceMode2D.Impulse);
        rb.AddTorque(turningForce, ForceMode2D.Impulse);
        throwPickUpCooldownEndTime = Time.time + throwPickUpCooldown;
    }

    public void Spawned(Weapon weapon)
    {
        InitGroundWeapon(weapon);
    }

    public WeaponPreset.WeaponSlot GetWeaponSlot()
    {
        return weapon.WeaponStats.weaponSlot;
    }

    public bool CanBePickedUp()
    {
        return Time.time > throwPickUpCooldownEndTime;
    }

    public override Weapon OnPickUp()
    {
        return weapon;
    }

    public override void OnPickedUp()
    {
        Destroy(gameObject);
    }
}
