using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour, ITakeFix, IInteractable
{
    [SerializeField] private int _requiredFix;
    [SerializeField] private bool isFixed;
    [SerializeField] private Sprite _fixedSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public void TakeFix(int fixForce)
    {
        _requiredFix -= fixForce;
        if(_requiredFix <= 0)
        {
            FixExit();
        }
    }
    public void FixExit()
    {
        isFixed = true;
        _spriteRenderer.sprite = _fixedSprite;
    }
    public void EndLevel()
    {
        Debug.Log("LevelEnd");
    }

    public void Interact()
    {
        if (isFixed) EndLevel();

    }
    
}
