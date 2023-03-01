using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class ItemContainer : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private Collider2D _triggerCollider;
    private void Awake()
    {
        _triggerCollider = this.GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if(collision.gameObject.TryGetComponent<PlayableCharacter>(out PlayableCharacter player))
        {
            player.TakeItem(_item);
            Destroy(this.gameObject);
        }
    }
}
