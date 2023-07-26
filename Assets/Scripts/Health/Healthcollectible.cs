using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthcollectible : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float healthvalue;
    [SerializeField] private bool isHealthRefill;
    [SerializeField] private bool increaseMaxHealth;
    [SerializeField] private float maxHealthIncreaseValue;
    [SerializeField] private AudioClip pickupSound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SoundManager.instance.Playsound(pickupSound);
            Health playerHealth = col.GetComponent<Health>();

            if (isHealthRefill)
            {
                playerHealth.Addheal(healthvalue); // Hồi lại máu tối đa
            }
            else if (increaseMaxHealth)
            {
                playerHealth.IncreaseMaxHealth(maxHealthIncreaseValue); // Tăng máu tối đa
            }

            playerHealth.Addheal(healthvalue); // Hồi lại máu

            gameObject.SetActive(false);
        }
    }
}
