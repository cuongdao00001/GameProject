using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamaged // Will damage player every time
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator anim;
    private bool hit;
    private BoxCollider2D boxCol;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }
    public void ActiveProjectitle()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        boxCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        hit = true;
        base.OnTriggerEnter2D(col); //Execute logic from parent script first
        boxCol.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode"); //When the object is a fireball explode it
        else
            gameObject.SetActive(false); //When this hits any object deactivate arrow
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
