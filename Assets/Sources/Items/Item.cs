using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName = "Item")]
public class Item : ScriptableObject, IPickable
{
    public string title;
    [TextArea]
    public string description;
    public Sprite sprite;


    public int maxHealthImpact;
    public int currentHealthImpact;
    public int tapeCountImpact;
    public int speedMuliplier;
    public int damageMultiplierImpact;
    public void PickUp()
    {
        
    }

}
