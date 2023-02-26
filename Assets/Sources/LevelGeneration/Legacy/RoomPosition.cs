using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPosition : MonoBehaviour 
{
    [SerializeField] private LayerMask _roomLayerMask;
    

    public GameObject _fillerRoom;
    
    public void CheckForEmptyRoomPosition()
    {
        Collider2D roomCollider = Physics2D.OverlapCircle(transform.position, 1, _roomLayerMask);
        if (roomCollider == null)
        {
            FillRoomPosition(_fillerRoom);
        }
    }

    private void FillRoomPosition(GameObject roomCollider)
    {
        Instantiate(roomCollider, transform.position, Quaternion.identity);
    }
}
