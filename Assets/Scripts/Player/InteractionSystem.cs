using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // Detected Point
    public Transform detectionPoint;
    // Detected Radius
    private const float detectionRadius = 0.2f;
    // Dectected Layer
    public LayerMask detectionLayer;
    // Cached trigger object
    public GameObject detectedObject;
    public GameObject grabbedObject;
    public float grabbedobjectyvalue;
    public Transform grabPoint;

    [Header("Examination Parameters")]
    // Examine window
    public GameObject examine;
    public Image examineImage;
    public TextMeshProUGUI examineText;
    public bool isExamining;
    public bool isGrabbing;

    [Header("Others")]
    public PlayerMove playerMove;

    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();

    }

    // Update is called once per frame
    void Update()
    {
        if (DetectObject())
        {
            if (InteractInput())
            {
                // If we grabbed something don't interact with other items, drop the grabbed items first
                if (isGrabbing)
                {
                    GrapDrop();
                    return;
                }

                if (!playerMove.isRunning)
                {
                    detectedObject.GetComponent<Item>().Interact();
                }
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,detectionRadius,detectionLayer);
        if (obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position,detectionRadius);
    }

    public void ExamineItem(Item _item)
    {
        if (isExamining)
        {
            // Hide the examine
            examine.SetActive(false);
            // Enable the boolean
            isExamining = false;
        }
        else
        {
            // Show the item's image in the midle
            examineImage.sprite = _item.GetComponent<SpriteRenderer>().sprite;
            // Write description text underneath the image
            examineText.text = _item.descriptionText;
            // Display an examine window
            examine.SetActive(true);
            // Enable the boolean
            isExamining = true;
        }
    }

    public void GrapDrop()
    {
        // Check if we do have a grabbed object => drop it
        if (isGrabbing)
        {
            // Make isGrabbing false 
            isGrabbing = false;
            // Unparent the grabbed object
            grabbedObject.transform.parent = null;
            // Set the y position to its origin
            grabbedObject.transform.position =
                new Vector3(grabbedObject.transform.position.x, grabbedobjectyvalue, grabbedObject.transform.position.z);
            // Null the grabbed object reference
            grabbedObject = null;
        }

        // Check if we have nothing grabbed grab the detected item
        else
        {
            // Enable the isGrabbing bool
            isGrabbing = true;
            // Assign the grabbed object to the object itself
            grabbedObject = detectedObject;
            // Parent the grabbed object to the player
            grabbedObject.transform.parent = transform;
            // Cache the y value of object 
            grabbedobjectyvalue = grabbedObject.transform.position.y;
            // Adjust the position of the grabbed object to be coloser hands
            grabbedObject.transform.localPosition = grabPoint.localPosition;
        }
    }
}
