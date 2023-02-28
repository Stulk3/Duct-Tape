using System;
using UnityEngine;
using UnityEngine.Animations;

public class PlayableCharacter : Character
{
    [SerializeField] private int _tapeCount;
    private Animator _animator;
    public GameObject upperBody;
    public GameObject lowerBody;

    public event Action<int> onHealthChanged;
    public event Action<int> onTapeChanged;
    private void Start()
    {
        onHealthChanged?.Invoke(currentHealth);
        onTapeChanged?.Invoke(_tapeCount);
    }
    public override void Move(Vector2 moveInput)
    {
        characterRigidbody.velocity = moveInput * speed;
    }
    public override void Stop()
    {
        characterRigidbody.velocity = new Vector2(0, 0);
    }

    public override void TakeFix(int fixForce)
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += fixForce;
            onHealthChanged?.Invoke(currentHealth);
        }
        else
        {
            currentHealth = maxHealth;
            onHealthChanged?.Invoke(currentHealth);
        }
        
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
    public static PlayableCharacter operator +(PlayableCharacter a, Item b)
    {
        return new PlayableCharacter()
        {
            _tapeCount = a._tapeCount + b.tapeCountImpact
        };
    }

    public override void Die()
    {
        onHealthChanged?.Invoke(0);
    }

    public void TakeItem(Item item)
    {
        maxHealth += item.maxHealthImpact;
        currentHealth += item.currentHealthImpact;
        _tapeCount += item.tapeCountImpact;
        speed += item.speedMuliplier;
        damageMultiplier = item.damageMultiplierImpact;

        onHealthChanged?.Invoke(currentHealth);
        onTapeChanged?.Invoke(_tapeCount);
        /// Заглушка, правильнее будет отдельно создать Класс Стат и уже через Провайдеры плюсовать Стат персонажа и Стат Предмета
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
            TakeDamage(2);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            TakeFix(2);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Item item = ItemDatabase.instance.GetRandomItemFromAvailableItems();
            Debug.Log(item);
            TakeItem(item);
        }
    }
#endif
}
