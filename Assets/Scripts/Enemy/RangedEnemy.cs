﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Range Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballsound;

    [Header("Others")]
    public int coinValue = 10; // Số điểm được cộng sau khi giết enemy
    private ScoreSystem scoreSystem;

    // References
    private Animator aim;
    private EnemyPatrol enemyPatrol;
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        aim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Attack only when player in sight
        if (CanAttackPlayer())
        {
            cooldownTimer = 0;
            aim.SetTrigger("Ecast");
            RangedAttack();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool CanAttackPlayer()
    {
        return cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0 && PlayerInSight();
    }

    private void RangedAttack()
    {
        SoundManager.instance.Playsound(fireballsound);
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActiveProjectitle();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void Die()
    {
        // Xử lý khi enemy bị giết
        if (scoreSystem != null)
        {
            scoreSystem.AddScore(coinValue); // Gọi hàm AddScore để tăng điểm
        }
        Destroy(gameObject); // Hoặc thực hiện xử lý khác khi enemy bị giết
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Die();
        }
    }

}
