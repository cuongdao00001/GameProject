using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, Pickup, Examine, GrabDrop }
    public enum ItemType { Static, Consumables }

    [Header("Attribute")]
    public InteractionType interactType;
    public ItemType type;
    public bool stackable = false;

    [Header("Examine")]
    public string descriptionText;
    public Sprite image;

    [Header("Custome Events")]
    public UnityEvent customEvent;
    public UnityEvent consumeEvent;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10; 
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.Pickup:
                if (!FindObjectOfType<InventorySystem>().CanPickUp())
                    return;
                // Add the object to the pickedupitems list
                FindObjectOfType<InventorySystem>().Pickup(gameObject);
                // Disable 
                gameObject.SetActive(false);
                break;

            case InteractionType.Examine:
                // Call the examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                break;

            case InteractionType.GrabDrop:
                // Grab interaction
                FindObjectOfType<InteractionSystem>().GrapDrop();
                break;

            default:
                Debug.Log("Null Item");
                break;
        }

        // Invoke the custom event(s)
        customEvent.Invoke();
    }

}
