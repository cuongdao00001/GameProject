using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [System.Serializable]
    // Inventory item class
    public class InventoryItem
    {
        public GameObject obj;
        public int stack = 1;

        public InventoryItem(GameObject o, int s = 1)
        {
            obj = o;
            stack = s;
        }
    }

    [Header("General Fields")]
    // List of items picked up
    public List<InventoryItem> items = new List<InventoryItem>();
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
        // If item is stackable
        if (_item.GetComponent<Item>().stackable)
        {
            // Check if we have an exitsting item in our inventory
            InventoryItem existingItem = items.Find(x => x.obj.name == _item.name);
            // If yes stack it
            if (existingItem != null)
            {
                existingItem.stack++;
            }
            // If no add it as a new item
            else
            {
                InventoryItem i = new InventoryItem(_item);
                items.Add(i);
            }
        }
        // If item isn't stackable
        else
        {
            InventoryItem i = new InventoryItem(_item);
            items.Add(i);
        }
        Update_UI();
        
    }

    public bool CanPickUp()
    {
        if (items.Count >= items_Images.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Refresh the UI elements in the inventory window
    void Update_UI()
    {
        Hideall();
        // For each items in the items list
        // Show it in the respective slot in the items image
        for (int i = 0; i < items.Count; i++)
        {
            items_Images[i].sprite = items[i].obj.GetComponent<SpriteRenderer>().sprite;
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
        if (items[_id].stack == 1)
            // If stack == 1 write only name
            description_Title.text = items[_id].obj.name;

        else
            // If stack > 1 write name + x stackvalue
            description_Title.text = items[_id].obj.name + " x" + items[_id].stack;
        // Show the description
        description_Description.text = items[_id].obj.GetComponent<Item>().descriptionText;
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
        if (items[_id].obj.GetComponent<Item>().type == Item.ItemType.Consumables)
        {
            Debug.Log($"Consumed {items[_id].obj.name}");
            // Invoke the consume custome event
            items[_id].obj.GetComponent<Item>().consumeEvent.Invoke();
            // Reduce the stack number
            items[_id].stack--;
            // If the stack is zero
            if (items[_id].stack == 0)
            {
                // Clear the item from the list
                items.RemoveAt(_id);
                // Destroy the item from the list
                Destroy(items[_id].obj, 0.1f);
            }
            // Update Ui
            Update_UI();
        }
    }
}
