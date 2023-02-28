using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Character
{
    public override void Die()
    {
        Destroy(this.gameObject);
    }

    public override void Move(Vector2 moveInput)
    {

    }

    public override void Stop()
    {
 
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void TakeFix(int fixForce)
    {
       
    }
}
