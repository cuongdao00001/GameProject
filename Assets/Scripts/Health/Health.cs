using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int iFrameCount;
    private SpriteRenderer spriteRend;

    [Header("Component")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
              
    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Inuvunerability());
            SoundManager.instance.Playsound(hurtSound);
        }
        else
        {
            if(!dead) 
            {               
                //Deactivate all attached component classes
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded",true);
                anim.SetTrigger("die");

                dead = true;
                SoundManager.instance.Playsound(deathSound);
            }        
        }
    }
    public void Addheal(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);

    }

    public void IncreaseMaxHealth(float _value)
    {
        float previousMaxHealth = currentHealth - startingHealth;
        startingHealth += _value;
        currentHealth = startingHealth + previousMaxHealth;
    }

    public void Setheal(float _value)
    {
        startingHealth = _value;
        currentHealth = _value;
    }

    public void Respawn()
    {
        dead = false;
        Addheal(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Inuvunerability());

        // Activate all attached component classes
        foreach (Behaviour component in components)
            component.enabled = true;
    }
    private IEnumerator Inuvunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < iFrameCount; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (iFrameCount * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
