using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    [SerializeField] public int damage;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;
    private int originalDamage; // The original damage before applying the buff
    private bool damageBuffActive = false;
    private float damageBuffEndTime;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    private Animator aim;
    private PlayerMove playerMove;
    private float coolDownTimer = Mathf.Infinity;

    private void Start()
    {
        aim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && coolDownTimer > attackCoolDown && playerMove.CanAttack())
            Attack();

        coolDownTimer += Time.deltaTime;

        if (damageBuffActive && Time.time >= damageBuffEndTime)
        {
            RemoveDamageBuff(originalDamage);
        }
    }

    private void Attack()
    {
        SoundManager.instance.Playsound(attackSound);
        aim.SetTrigger("attack");
        playerMove.DisableMovement(); // Vô hiệu hóa di chuyển của player trong thời gian tấn công
        StartCoroutine(EnableMovementAfterAttack()); // Kích hoạt lại di chuyển sau khi tấn công hoàn thành
        coolDownTimer = 0;
    }

    private IEnumerator EnableMovementAfterAttack()
    {
        yield return new WaitForSeconds(1.1f); // Đợi cho đến khi animation attack hoàn thành (thời gian của animation attack)

        playerMove.EnableMovement(); // Kích hoạt lại di chuyển của player
    }

    public void DamageEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                enemy.GetComponent<Animator>().SetTrigger("hurt");

                // Áp dụng knock back
                ApplyKnockback(enemy.GetComponent<Rigidbody2D>());
            }
        }
    }

    private void ApplyKnockback(Rigidbody2D enemyRigidbody)
    {
        Vector2 knockbackDirection = (enemyRigidbody.transform.position - transform.position).normalized;
        enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void ApplyDamageBuff(int damageIncreaseAmount, float duration)
    {
        damageBuffActive = true;
        originalDamage = damage; // Store the original damage
        damage += damageIncreaseAmount; // Apply the buff
        damageBuffEndTime = Time.time + duration;
    }

    public void RemoveDamageBuff(int previousDamage)
    {
        damageBuffActive = false;
        damage = previousDamage; // Restore the original damage
    }

}
