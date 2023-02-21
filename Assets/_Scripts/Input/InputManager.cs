using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions playerActions;

    public static Vector2 moveDirection;
    public static Vector2 mousePosition;
    public static Vector2 mouseDelta;

    public static event UnityAction OnFirePerformed;
    public static event UnityAction OnFireCanceled;

    public static event UnityAction OnReloadPerformed;

    public static event UnityAction OnSelectPrimaryPerformed;
    public static event UnityAction OnSelectSecondaryPerformed;
    public static event UnityAction OnSelectMeleePerformed;

    public static event UnityAction OnThrowWeaponPerformed;

    public static event UnityAction OnInteractPerformed;

    //Movement
    public static event UnityAction OnWalkPerformed;
    public static event UnityAction OnWalkingCanceled;

    private void Awake()
    {
        playerActions = new PlayerInputActions();
        playerActions.Player.Enable();
    }

    private void OnEnable()
    {

        playerActions.Player.MousePosition.performed += OnMousePosition;
        playerActions.Player.MouseDelta.performed += OnMouseDelta;
        playerActions.Player.MouseDelta.canceled += OnMouseDelta;

        playerActions.Player.Fire.performed += OnFire;
        playerActions.Player.Fire.canceled += OnFire;

        playerActions.Player.Reload.performed += OnReload;

        playerActions.Player.SelectPrimary.performed += OnSelectPrimary;
        playerActions.Player.SelectSecondary.performed += OnSelectSecondary;
        playerActions.Player.SelectMelee.performed += OnSelectMelee;

        playerActions.Player.ThrowWeapon.performed += OnThrowWeapon;

        playerActions.Player.Interact.performed += OnInteract;

        //Movement
        playerActions.Player.Move.performed += OnMove;
        playerActions.Player.Move.canceled += OnMove;
        playerActions.Player.Walk.performed += OnWalk;
        playerActions.Player.Walk.canceled += OnWalk;
    }

    private void OnDisable()
    {

        playerActions.Player.MousePosition.performed -= OnMousePosition;
        playerActions.Player.MouseDelta.performed -= OnMouseDelta;
        playerActions.Player.MouseDelta.canceled -= OnMouseDelta;

        playerActions.Player.Fire.performed -= OnFire;
        playerActions.Player.Fire.canceled -= OnFire;

        playerActions.Player.Reload.performed -= OnReload;

        playerActions.Player.SelectPrimary.performed -= OnSelectPrimary;
        playerActions.Player.SelectSecondary.performed -= OnSelectSecondary;
        playerActions.Player.SelectMelee.performed -= OnSelectMelee;

        playerActions.Player.ThrowWeapon.performed -= OnThrowWeapon;

        playerActions.Player.Interact.performed -= OnInteract;

        //Movement
        playerActions.Player.Move.performed -= OnMove;
        playerActions.Player.Move.canceled -= OnMove;
        playerActions.Player.Walk.performed -= OnWalk;
        playerActions.Player.Walk.canceled -= OnWalk;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }

    private void OnMousePosition(InputAction.CallbackContext ctx)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
    }

    private void OnMouseDelta(InputAction.CallbackContext ctx)
    {
        mouseDelta = ctx.ReadValue<Vector2>();
    }
    
    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnFirePerformed?.Invoke();
        if (ctx.canceled) OnFireCanceled?.Invoke();
    }

    private void OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnReloadPerformed?.Invoke();
    }

    private void OnSelectPrimary(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnSelectPrimaryPerformed?.Invoke();
    }

    private void OnSelectSecondary(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnSelectSecondaryPerformed?.Invoke();
    }

    private void OnSelectMelee(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnSelectMeleePerformed?.Invoke();
    }

    private void OnThrowWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnThrowWeaponPerformed?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnInteractPerformed?.Invoke();
    }

    private void OnWalk(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnWalkPerformed?.Invoke();
        if (ctx.canceled) OnWalkingCanceled?.Invoke();
    }
}
