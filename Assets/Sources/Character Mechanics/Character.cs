using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Character : MonoBehaviour, ITakeDamage, ITakeFix, IMortal
{
    public int initialHealth;
    public int maxHealth;
    public int currentHealth;
    public float speed;
    public float damageMultiplier;
    public Rigidbody2D characterRigidbody;

    private void Awake()
    {
        currentHealth = initialHealth;
        characterRigidbody = this.GetComponent<Rigidbody2D>();
    }
    public abstract void TakeDamage(int damage);

    public abstract void TakeFix(int fixForce);

    public abstract void Die();

    public abstract void Move(Vector2 moveinput);
    public abstract void Stop();
}
