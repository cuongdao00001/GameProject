using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;   
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    // References
    private Animator aim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    void Start()
    {
        aim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (CanAttackPlayer())
        {
            cooldownTimer = 0;
            aim.SetTrigger("Eattack");
            SoundManager.instance.Playsound(attackSound);
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool CanAttackPlayer()
    {
        return cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0 && PlayerInSight();
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        // Player in range
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
