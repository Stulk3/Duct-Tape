using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDatabase : MonoBehaviour
{
    ///Singleton
    public static ItemDatabase instance;

    [SerializeField] Item[] _availableItems;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public Item GetRandomItemFromAvailableItems()
    {
        int randomIndex = Random.Range(0, _availableItems.Length);
        return _availableItems[randomIndex];
    }
}
