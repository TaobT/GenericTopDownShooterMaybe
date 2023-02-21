using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour
{
    public enum WeaponState
    {
        Idle,
        Shooting,
        Reloading,
        Equiping
    }

    private WeaponState currentState;

    [SerializeField] private WeaponPreset initialPrimaryWeapon;
    [SerializeField] private WeaponPreset initialSecondaryWeapon;
    [SerializeField] private WeaponPreset initialMeleeWeapon;
    private Weapon primaryWeapon = new Weapon();
    private Weapon secondaryWeapon = new Weapon();
    private Weapon meleeWeapon = new Weapon();
    [SerializeField] private GameObject groundWeaponPf;

    [Space]
    [SerializeField] private BulletTracerController bulletTracer;

    [Header("Movement Effect")]
    [SerializeField] private PlayerMovement playerMovement;

    private Weapon activeWeapon = new Weapon();

    private void OnEnable()
    {
        InputManager.OnFirePerformed += StartShooting;
        InputManager.OnFireCanceled += StopShooting;

        InputManager.OnReloadPerformed += StartReloading;

        InputManager.OnSelectPrimaryPerformed += SelectPrimaryWeapon;
        InputManager.OnSelectSecondaryPerformed += SelectSecondaryWeapon;
        InputManager.OnSelectMeleePerformed += SelectMeleeWeapon;

        InputManager.OnThrowWeaponPerformed += ThrowActiveWeapon;
    }

    private void OnDisable()
    {
        InputManager.OnFirePerformed -= StartShooting;
        InputManager.OnFireCanceled -= StopShooting;

        InputManager.OnReloadPerformed -= StartReloading;

        InputManager.OnSelectPrimaryPerformed -= SelectPrimaryWeapon;
        InputManager.OnSelectSecondaryPerformed -= SelectSecondaryWeapon;
        InputManager.OnSelectMeleePerformed -= SelectMeleeWeapon;

        InputManager.OnThrowWeaponPerformed -= ThrowActiveWeapon;
    }

    private void Start()
    {
        InitialWeapons();

        SelectMeleeWeapon();
    }

    private void Update()
    {
        if (activeWeapon.IsEmpty()) return;

        ApplyAngleDecrement();

        RecoilAngleCalculation();

        UpdateState();
    }

    private void InitialWeapons()
    {
        if (initialPrimaryWeapon != null) primaryWeapon = new Weapon(initialPrimaryWeapon);
        if (initialSecondaryWeapon != null) secondaryWeapon = new Weapon(initialSecondaryWeapon);
        if (initialMeleeWeapon != null) meleeWeapon = new Weapon(initialMeleeWeapon);
    }

    #region State
    private void UpdateState()
    {
        switch (currentState)
        {
            case WeaponState.Idle:
                break;
            case WeaponState.Shooting:
                ShootActiveWeapon();
                break;
            case WeaponState.Reloading:
                ReloadWeapon();
                break;
            case WeaponState.Equiping:
                EquipWeapon();
                break;
        }
    }

    private void SwitchState(WeaponState newState)
    {
        //If newState priority is less than currentState priority, return
        if (newState < currentState) return;

        switch (currentState)
        {
            case WeaponState.Shooting:
                StopShooting();
                break;
            case WeaponState.Reloading:
                StopReloading();
                break;
            case WeaponState.Equiping:
                StopEquipingWeapon();
                break;
        }
        currentState = newState;

    }

    private void SwitchStateToIdle()
    {
        currentState = WeaponState.Idle;
    }
    #endregion

    #region Shooting Related
    private float nextShootTime;
    //Shoot Weapon
    private void ShootActiveWeapon()
    {
        //Has ammo or is a melee weapon, Melee weapons doesn't have ammo
        //and fire rate is ready
        if (Time.time > nextShootTime)
        {

            if (activeWeapon.WeaponHasAmmo() || activeWeapon.IsMelee())
            {
                //Shoot SFX
                SoundManager.Instance.PlayClipOnTransform(activeWeapon.WeaponStats.fire, transform);
                if(!activeWeapon.IsMelee())//Ammo weapons logic
                {

                    float bulletAngle = GetBulletAngle();

                    Vector3 startPos = transform.position + transform.right * activeWeapon.WeaponStats.firingPoint;
                    Vector3 endPos = startPos + Quaternion.Euler(0, 0, bulletAngle) * transform.right * 25;

                    bulletTracer.DrawTrace(startPos, endPos);

                    ApplyAngleIncrement();

                    activeWeapon.RemoveAmmo();

                    //If is semi wait until start shooting is pressed again
                    if(activeWeapon.WeaponStats.firingType == WeaponPreset.FiringType.SemiAutomatic)
                    {
                        SwitchStateToIdle();
                    }
                    //Debug.Log("Weapon shooted! Bullet angle: " + bulletAngle + "  Firing Angle After Shoot: " + currentFiringAngle + " Current Ammo: " + activeWeapon.CurrentAmmo);
                    Debug.Log("<color=#ffa500>"+ activeWeapon.WeaponStats.weaponName +" Shooted!</color>");
                }
                else
                {
                    Debug.Log("<color=#ffa500>" + activeWeapon.WeaponStats.weaponName + " Melee!</color>");
                }
            }
            else //Empty mag
            {
                //Play Empty Magazine SFX
                SoundManager.Instance.PlayClipOnTransform(activeWeapon.WeaponStats.empty, transform);
            }
            nextShootTime = Time.time + (1 / activeWeapon.WeaponStats.firePerSecond);
        }
    }

    private void StartShooting()
    {
        SwitchState(WeaponState.Shooting);
    }

    private void StopShooting()
    {
        if(currentState == WeaponState.Shooting)
        {
            SwitchStateToIdle();
        }
    }

    #region Recoil
    //Recoil
    private float firingRecoilAngle;
    private float totalRecoilAngle;

    //Generates a random value from 0 to currentRecoilAngle
    //The the half of currentRecoilAngle is substracted so 0° is always in the middle
    private float GetBulletAngle()
    {
        return Random.Range(0, totalRecoilAngle) - (totalRecoilAngle/2);
    }

    private void ApplyAngleIncrement()
    {
        firingRecoilAngle += activeWeapon.WeaponStats.angleIncrement;
        firingRecoilAngle = Mathf.Clamp(firingRecoilAngle, activeWeapon.WeaponStats.minFiringAngle, activeWeapon.WeaponStats.maxFiringAngle);
    }

    private void ApplyAngleDecrement()
    {
        if (currentState != WeaponState.Shooting) return;
        firingRecoilAngle -= activeWeapon.WeaponStats.angleDecrement * Time.deltaTime;
        firingRecoilAngle = Mathf.Clamp(firingRecoilAngle, activeWeapon.WeaponStats.minFiringAngle, activeWeapon.WeaponStats.maxFiringAngle);
    }

    private void SetMinFiringAngle()
    {
        firingRecoilAngle = activeWeapon.WeaponStats.minFiringAngle;
    }

    private void RecoilAngleCalculation()
    {
        totalRecoilAngle = firingRecoilAngle + (firingRecoilAngle * activeWeapon.WeaponStats.movementInaccuracyFactor * playerMovement.GetSpeedRatio());
    }

    public float GetTotalRecoilAngle()
    {
        return totalRecoilAngle;
    }

    #endregion

    #endregion

    #region Reloading
    private float nextReloadTime;
    private int reloadSFXIndex;

    private void ReloadAmmoLogic()
    {
        switch (activeWeapon.WeaponStats.reloadingType)
        {
            case WeaponPreset.ReloadingType.Magazine:
                activeWeapon.SetAmmo(activeWeapon.CurrentAmmo + activeWeapon.RemoveAmmoFromReserve(activeWeapon.WeaponStats.maxAmmo - activeWeapon.CurrentAmmo));
                Debug.Log("Magazine fully reloaded. Current bullets: " + activeWeapon.CurrentAmmo);
                break;
            case WeaponPreset.ReloadingType.PerBullet:
                activeWeapon.SetAmmo(activeWeapon.CurrentAmmo + activeWeapon.RemoveAmmoFromReserve(activeWeapon.WeaponStats.bulletsReloaded));
                Debug.Log("Reloaded " + activeWeapon.WeaponStats.bulletsReloaded + " bullets, Current bullets: " + activeWeapon.CurrentAmmo);
                break;
        }
    }

    private void ReloadWeapon()
    {
        if(Time.time > nextReloadTime)
        {
            ReloadAmmoLogic();
            if (!activeWeapon.IsAmmoInReserve())//Stop reloading if reserve ammo is gone
            {
                StopReloading();
                Debug.Log("Reload stopped <color=#ffa500>" + activeWeapon.WeaponStats.weaponName + " reloaded with the last ammo in reserve</color>");
            }

            if (activeWeapon.WeaponHasFullAmmo()) //If is magazine based reloaded stop reloading
            {
                StopReloading();
                Debug.Log("<color=#ffa500>" + activeWeapon.WeaponStats.weaponName + " Fully Reloaded!</color>");
            }
            else// If not, keep reloading until the magazine is full or stop reloading
            {
                nextReloadTime = Time.time + activeWeapon.WeaponStats.reloadTime;
                Debug.Log("Reload in progress. Next reload time: " + nextReloadTime);
            }
        }
    }

    private void StartReloading()
    {
        if (activeWeapon.IsMelee() || !activeWeapon.IsAmmoInReserve() || activeWeapon.WeaponHasFullAmmo()) return;

        //Play Reload SFX
        reloadSFXIndex = SoundManager.Instance.PlayDependantClipOnTransform(activeWeapon.WeaponStats.reload, transform);

        nextReloadTime = Time.time + activeWeapon.WeaponStats.reloadTime;
        SwitchState(WeaponState.Reloading);
        Debug.Log("Reload started. Next reload time: " + nextReloadTime);
    }

    private void StopReloading()
    {
        if(currentState == WeaponState.Reloading)
        {
            SwitchStateToIdle();
            SoundManager.Instance.StopDependantClip(reloadSFXIndex);
        }
    }
    #endregion

    #region Select Weapon
    //Select Weapon
    private float nextEquipTime;
    private int cockingSFXIndex;
    private void ChangeWeapon(Weapon weapon)
    {
        if (activeWeapon == weapon) return;

        activeWeapon = weapon;

        StartEquipingWeapon();
        SetMinFiringAngle();
        //Set player movement speed
        playerMovement.SetWeaponMovementSpeed(activeWeapon.WeaponStats.playerSpeed);
    }

    private void EquipWeapon()
    {
        if(Time.time > nextEquipTime)
        {
            SwitchStateToIdle();
            Debug.Log("Weapon equiped");
        }
    }

    private void StartEquipingWeapon()
    {
        //Play Cocking SFX
        cockingSFXIndex = SoundManager.Instance.PlayDependantClipOnTransform(activeWeapon.WeaponStats.equip, transform);

        nextEquipTime = Time.time + activeWeapon.WeaponStats.equipTime;
        SwitchState(WeaponState.Equiping);
        Debug.Log("Equiping weapon. Next equip time: " + nextEquipTime);
    }

    private void StopEquipingWeapon()
    {
        SoundManager.Instance.StopDependantClip(cockingSFXIndex);
        SwitchStateToIdle();
    }

    private void SelectPrimaryWeapon()
    {
        if (primaryWeapon.IsEmpty()) return;
        Debug.Log("Selecting Primary Weapon...");
        ChangeWeapon(primaryWeapon);
    }

    private void SelectSecondaryWeapon()
    {
        if (secondaryWeapon.IsEmpty()) return;
        Debug.Log("Selecting Secondary Weapon...");
        ChangeWeapon(secondaryWeapon);
    }

    private void SelectMeleeWeapon()
    {
        if (meleeWeapon.IsEmpty()) return;
        Debug.Log("Selecting Melee Weapon...");
        ChangeWeapon(meleeWeapon);
    }

    #endregion

    #region Pickup, Throw and Purchase Weapon
    //If weapon is picked up gets current weapon ammo
    public void PickUpWeapon(Weapon newWeapon, bool replacing = true)
    {
        if (replacing)
        {
            if(newWeapon.WeaponStats.weaponSlot == activeWeapon.WeaponStats.weaponSlot) //Is replacing the active weapon?
            {
                ChangeWeapon(newWeapon);//Selects the new weapon
            }

            ThrowWeaponInSlot(newWeapon.WeaponStats.weaponSlot, false);
        }

        switch (newWeapon.WeaponStats.weaponSlot)
        {
            case WeaponPreset.WeaponSlot.Primary:
                primaryWeapon = newWeapon;
                break;
            case WeaponPreset.WeaponSlot.Secondary:
                secondaryWeapon = newWeapon;
                break;
        }
    }

    private void ThrowActiveWeapon()
    {
        if(activeWeapon.WeaponStats.weaponSlot == WeaponPreset.WeaponSlot.Melee) return;
        ThrowWeaponInSlot(activeWeapon.WeaponStats.weaponSlot);
    }
    private void ThrowWeaponInSlot(WeaponPreset.WeaponSlot slot, bool switchWeapon = true)
    {
        bool isActive = (activeWeapon.WeaponStats.weaponSlot == slot);

        switch (slot)
        {
            case WeaponPreset.WeaponSlot.Primary:
                if (primaryWeapon.IsEmpty()) return;
                Debug.Log("Throwing Primary Weapon...");
                if (isActive && switchWeapon)
                {
                    if(!secondaryWeapon.IsEmpty()) SelectSecondaryWeapon();
                    else if (!meleeWeapon.IsEmpty()) SelectMeleeWeapon();
                }

                Instantiate(groundWeaponPf, transform.position, transform.rotation).GetComponent<GroundWeapon>().Throwed(primaryWeapon);
                primaryWeapon = new Weapon();
                break;
            case WeaponPreset.WeaponSlot.Secondary:
                if (secondaryWeapon.IsEmpty()) return;
                Debug.Log("Throwing Secondary Weapon...");
                if (isActive && switchWeapon)
                {
                    if (!primaryWeapon.IsEmpty()) SelectPrimaryWeapon();
                    else if (!meleeWeapon.IsEmpty()) SelectMeleeWeapon();
                }

                Instantiate(groundWeaponPf, transform.position, transform.rotation).GetComponent<GroundWeapon>().Throwed(secondaryWeapon);
                secondaryWeapon = new Weapon();
                break;
        }
    }

    public void PurchaseWeapon(WeaponPreset weapon)
    {

    }

    public bool SlotIsEmpty(WeaponPreset.WeaponSlot slot)
    {
        switch (slot)
        {
            case WeaponPreset.WeaponSlot.Primary:
                return primaryWeapon.IsEmpty();
            case WeaponPreset.WeaponSlot.Secondary:
                return secondaryWeapon.IsEmpty();
        }

        return false;
    }

    #endregion

}


public static class PlayerWeaponsControllerEvents
{

}