using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    private Rigidbody2D rb;
    [SerializeField] private float defaultMaxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField][Range(0f, 1f)] private float walkingMod;
    private Vector2 lastDirection;
    private float maxSpeed;
    private float currentSpeed;
    private bool isWalking;

    private void OnEnable()
    {
        InputManager.OnWalkPerformed += StartWalking;
        InputManager.OnWalkingCanceled += StopWalking;
    }

    private void OnDisable()
    {
        InputManager.OnWalkPerformed -= StartWalking;
        InputManager.OnWalkingCanceled -= StopWalking;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        maxSpeed = defaultMaxSpeed;
    }

    private void FixedUpdate()
    {
        Move(InputManager.moveDirection);
    }

    private void Move(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            Acceleration();
            lastDirection = direction;
        }
        else
        {
            Deceleration();
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        float movementSpeed = isWalking ? currentSpeed * walkingMod : currentSpeed;
        rb.MovePosition(rb.position + (lastDirection * movementSpeed) * Time.fixedDeltaTime);
    }

    private void Acceleration()
    {

        currentSpeed += acceleration * Time.fixedDeltaTime;
    }

    private void Deceleration()
    {
        currentSpeed -= deceleration * Time.fixedDeltaTime;
    }

    public void SetWeaponMovementSpeed(float playerSpeed)
    {
        maxSpeed = playerSpeed;
    }

    private void StartWalking()
    {
        isWalking = true;
    }

    private void StopWalking()
    {
        isWalking = false;
    }

    public float GetSpeedRatio()
    {
        return currentSpeed / maxSpeed;
    }
}
