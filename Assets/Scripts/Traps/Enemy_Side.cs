using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Side : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float moveDistance;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    void Start()
    {
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            col.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
