using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Patrol points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator aim;
    void Start()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        aim.SetBool("Emoving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveinDirection(-1);
            else
                DirectionChange();           
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveinDirection(1);
            else            
                DirectionChange();
            
        }        
    }

    private void DirectionChange()
    {
        aim.SetBool("Emoving", false);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;

    }

    private void MoveinDirection(float _direction)
    {
        idleTimer = 0;
        aim.SetBool("Emoving",true);
        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) *  _direction, initScale.y, initScale.z);
        // Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);

    }
}
