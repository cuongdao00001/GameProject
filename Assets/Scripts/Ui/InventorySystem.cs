using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    // List of items picked up
    public List<GameObject> items = new List<GameObject>();
    // Flag indicates if the inventory open or not
    public bool isOpen;

    [Header("Ui Items Section")]
    // Inventory system
    public GameObject ui_Window;
    public Image[] items_Images;

    [Header("Ui Items Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Description;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            toggleInventory();
        }
    }

    void toggleInventory()
    {
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
        Update_UI();

        if (isOpen)
        {
            FindObjectOfType<PlayerMove>().DisableMovement();
        }
        else
        {
            FindObjectOfType<PlayerMove>().EnableMovement();
        }       
    }

    // Add the item to the item list
    public void Pickup(GameObject _item)
    {
        items.Add(_item);
        Update_UI();
        
    }

    // Refresh the UI elements in the inventory window
    void Update_UI()
    {
        Hideall();
        // For each items in the items list
        // Show it in the respective slot in the items image
        for (int i = 0; i < items.Count; i++)
        {
            items_Images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            items_Images[i].gameObject.SetActive(true);
        }
    }

    // Hide all the item ui images
    void Hideall()
    {
        foreach (var i in items_Images)
        {
            i.gameObject.SetActive(false);
        }
        hideDescription();
    }

    public void showDescription(int _id)
    {
        // Set the image
        description_Image.sprite = items_Images[_id].sprite;
        // Set the title
        description_Title.text = items[_id].name;
        // Show the description
        description_Description.text = items[_id].GetComponent<Item>().descriptionText;
        // Set the window
        description_Image.gameObject.SetActive(true);
        description_Title.gameObject.SetActive(true);
        description_Description.gameObject.SetActive(true);
    }

    public void hideDescription()
    {
        description_Image.gameObject.SetActive(false);
        description_Title.gameObject.SetActive(false);
        description_Description.gameObject.SetActive(false);
    }

    public void consume (int _id)
    {
        if (items[_id].GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            Debug.Log($"Consumed {items[_id].name}");
            // Invoke the consume custome event
            items[_id].GetComponent<Item>().consumeEvent.Invoke();
            // Clear the item from the list
            items.RemoveAt(_id );
            // Destroy the item from the list
            Destroy(items[_id], 0.1f);
            // Update Ui
            Update_UI();
        }
    }
}
