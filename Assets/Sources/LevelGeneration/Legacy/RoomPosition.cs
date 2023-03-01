using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelGeneration;

public class RoomPosition : MonoBehaviour 
{
    [SerializeField] private LayerMask _roomLayerMask;
    

    public GameObject _fillerRoom;
    
    public void FillEmptyRoomPositions()
    {
        Collider2D roomCollider = Physics2D.OverlapCircle(transform.position, 1, _roomLayerMask);
        if (roomCollider == null)
        {
            FillRoomPosition(_fillerRoom);
        }
    }

    private void FillRoomPosition(GameObject room)
    {
        Instantiate(room, transform.position, Quaternion.identity);
        LevelGenerator.instance.AddRoomToSpawnedRooms(room);
    }
}
