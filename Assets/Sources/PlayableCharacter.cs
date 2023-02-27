using System;
using UnityEngine;
using UnityEngine.Animations;

public class PlayableCharacter : Character
{
    public event Action<int> onHealthChanged;
    private Animator _animator;
    public GameObject upperBody;
    public GameObject lowerBody;
    private void Start()
    {
        onHealthChanged?.Invoke(currentHealth);
    }
    public override void Move(Vector2 moveInput)
    {
        characterRigidbody.velocity = moveInput * speed;
    }
    public override void Stop()
    {
        characterRigidbody.velocity = new Vector2(0, 0);
    }

    public override void TakeFix(float fixForce)
    {
        currentHealth += 1;
    }

    public override void Die()
    {
        onHealthChanged?.Invoke(0);
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            onHealthChanged?.Invoke(currentHealth);
        }
    }

    private States State
    {
        get { return (States)_animator.GetInteger("State"); }
        set { _animator.SetInteger("State", (int)value); }
    }
    public enum States
    {
        Idle, Run, UsingTape
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }



#endif
}
