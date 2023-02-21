using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsController : MonoBehaviour
{
    [SerializeField] private PlayerSelector selector;
    [SerializeField] private PlayerWeaponsController weaponsController;

    private void OnEnable()
    {
        InputManager.OnInteractPerformed += InteractWithSelectedInteractable;
    }

    private void OnDisable()
    {
        InputManager.OnInteractPerformed -= InteractWithSelectedInteractable;
    }

    private void InteractWithSelectedInteractable()
    {
        Interactable selectedInteractable = selector.GetSelectedInteractable();
        if (selectedInteractable == null) return;
        Debug.Log("Interacting with " +selectedInteractable.name );
        switch (selectedInteractable.type)
        {
            case Interactable.InteractionType.Generic:
                selectedInteractable.OnInteraction();
                break;
            case Interactable.InteractionType.PickUp:
                weaponsController.PickUpWeapon(selectedInteractable.OnPickUp());
                selectedInteractable.OnPickedUp();
                break;
        }
    }
}
