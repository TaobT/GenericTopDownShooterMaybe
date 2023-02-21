using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Generic,
        PickUp
    }

    public abstract InteractionType type { get; }

    public virtual void OnInteraction()
    {

    }

    public virtual Weapon OnPickUp()
    {
        return null;
    }

    public virtual void OnPickedUp()
    {

    }
}
