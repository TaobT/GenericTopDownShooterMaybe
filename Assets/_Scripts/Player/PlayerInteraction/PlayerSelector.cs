using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] private LayerMask interactionsLayer;
    [SerializeField] private Transform selector; //Selects interactable from world
    [SerializeField] private Transform cursor; //Visual indicative of active interactable selected


    [Header("Selector")]
    [SerializeField] private Vector2 selectorDefaultPosition;
    [SerializeField] private float selectorRadius;
    [SerializeField] private float mouseSelectorRadius; //In which radius does the cursor interacts with mouse position?

    [Header("Cursor")]
    [SerializeField] private float cursorSpeed;

    [Header("Interactable")]
    [SerializeField] private float maxInteractableDistanceFromSelector; //From cursor position

    [Header("Virtual Mouse")]
    [SerializeField] private Transform virtualMouse;
    
    //Current state
    private Interactable selectedInteractable;

    private void Update()
    {
        CursorPosition();
        SelectorPosition();

        CheckInteractableInSelectorRadius();
        CheckInteractableDistanceFromSelector();
    }

    #region Cursor Position

    private void CursorPosition()
    {
        if (selectedInteractable != null)
        {
            cursor.position = Vector2.Lerp(cursor.position, selectedInteractable.transform.position, cursorSpeed * Time.deltaTime);
        }
        else if(IsMouseInSelectorRadius() && selectedInteractable == null)
        {
            cursor.position = Vector2.Lerp(cursor.position, selector.position, cursorSpeed * Time.deltaTime);
        }
        else
        {
            cursor.localPosition = Vector2.Lerp(cursor.localPosition, selectorDefaultPosition, cursorSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Selector Selection
    private void CheckInteractableInSelectorRadius()
    {
        Collider2D interactableCollider = Physics2D.OverlapCircle(selector.position, selectorRadius, interactionsLayer);
        if (interactableCollider != null)
        {
            SetSelectedInteractable(interactableCollider.GetComponent<Interactable>());
        }
    }

    private void CheckInteractableDistanceFromSelector()
    {
        if (selectedInteractable == null) return;
        if(Vector2.Distance(selectedInteractable.transform.position, selector.position) > maxInteractableDistanceFromSelector)
        {
            RemoveSelectedInteractable();
        }
    }

    private void SetSelectedInteractable(Interactable newInteractable)
    {
        if (selectedInteractable == newInteractable) return;
        selectedInteractable = newInteractable;
    }

    private void RemoveSelectedInteractable()
    {
        selectedInteractable = null;
    }
    #endregion

    #region Selector Position
    private void SelectorPosition()
    {
        if (IsMouseInSelectorRadius())
        {
            selector.position = virtualMouse.position;
        }
        else
        {
            selector.localPosition = selectorDefaultPosition;
        }
    }

    private bool IsMouseInSelectorRadius()
    {
        return (Vector2.Distance(virtualMouse.position, transform.position) < mouseSelectorRadius);
    }
    #endregion

    public Interactable GetSelectedInteractable()
    {
        return selectedInteractable;
    }

    private void OnDrawGizmos()
    {
        //Selecing with mouse radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mouseSelectorRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(selector.position, selectorRadius);

        if (selectedInteractable != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(selectedInteractable.transform.position, maxInteractableDistanceFromSelector);
        }
    }
}
