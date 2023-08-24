using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Effects/DamageBuff")]
public class DamageBuff : PowerupEffect
{
    public int damageIncrease;
    public float duration;

    private int originalDamage; // Store the original damage before applying the buff

    public override void Apply(GameObject _target)
    {
        PlayerAttack playerAttack = _target.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            originalDamage = playerAttack.damage; // Store the original damage
            playerAttack.ApplyDamageBuff(damageIncrease, duration);
        }
    }

    public override void Remove(GameObject _target)
    {
        PlayerAttack playerAttack = _target.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.RemoveDamageBuff(originalDamage);
        }
    }
}