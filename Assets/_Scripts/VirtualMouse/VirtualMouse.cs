using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This scripts creates a virtual mouse for the game
//The movement depends on the delta of the real mouse
public class VirtualMouse : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float sensibility;
    [Header("Virtual Mouse Bounds")]
    [SerializeField] private Vector2 minVirtualMousePosition;
    [SerializeField] private Vector2 maxVirtualMousePosition;
    [SerializeField] private Transform anchor; //Player position
    private void Update()
    {
        VirtualMouseMovement();
    }

    private void VirtualMouseMovement()
    {
        Vector3 movement = transform.position + (Vector3)InputManager.mouseDelta * sensibility;
        movement.x = Mathf.Clamp(movement.x, minVirtualMousePosition.x + anchor.position.x, maxVirtualMousePosition.x + anchor.position.x);
        movement.y = Mathf.Clamp(movement.y, minVirtualMousePosition.y + anchor.position.y, maxVirtualMousePosition.y + anchor.position.y);
        transform.position = movement;
    }
}
