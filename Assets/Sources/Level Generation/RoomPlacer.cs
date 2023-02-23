using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    [Header("Level Size")]
    [SerializeField] private Room[] _roomPresets;
    [SerializeField] private Room _startingRoom;
    [SerializeField] private int _roomCount;

    [Header("Level Size")]
    [SerializeField] private int _levelX;
    [SerializeField] private int _levelY;

    private Room[,] _placedRooms;

    private void Awake()
    {
        _placedRooms = new Room[_levelX, _levelY];
        _placedRooms[Center(_levelX), Center(_levelY)] = _startingRoom;
        
        PlaceRooms();
    }
    private void PlaceRooms()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int i = 0; i < _roomCount; i++)
        {
            PlaceRoom(vacantPlaces);
        }
    }
    private void PlaceRoom(HashSet<Vector2Int> vacantPlaces)
    {
        for(int x = 0; x < _placedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < _placedRooms.GetLength(1); y++)
            {
                if (_placedRooms[x, y] == null) continue;

                int maxX = _placedRooms.GetLength(0) - 1;
                int maxY = _placedRooms.GetLength(1) - 1;

                if (x > 0 && _placedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && _placedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x , y - 1));
                if (x > 0 && _placedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y > 0 && _placedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }


        SpawnRandomRoom(_roomPresets, vacantPlaces);

    }
    private bool ConnectToRoom(Room room, Vector2Int position)
    {
        int maxX = _placedRooms.GetLength(0) - 1;
        int maxY = _placedRooms.GetLength(1) - 1;

        List<Vector2Int> neighboringRooms = new List<Vector2Int>();

        if (room.doorUp != null && position.y < maxY && _placedRooms[position.x, position.y + 1]?.doorDown != null) neighboringRooms.Add(Vector2Int.up);
        if (room.doorDown != null && position.y > maxY && _placedRooms[position.x, position.y - 1]?.doorUp != null) neighboringRooms.Add(Vector2Int.down);
        if (room.doorRight != null && position.y < maxX && _placedRooms[position.x + 1, position.y]?.doorLeft != null) neighboringRooms.Add(Vector2Int.right);
        if (room.doorLeft != null && position.y > maxX && _placedRooms[position.x - 1, position.y]?.doorRight != null) neighboringRooms.Add(Vector2Int.left);

        if (neighboringRooms.Count == 0) return false;

        Vector2Int selectedDirection = neighboringRooms[Random.Range(0, neighboringRooms.Count)];
        Room selectedRoom = _placedRooms[position.x + selectedDirection.x, position.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.doorUp.SetActive(false);
            selectedRoom.doorDown.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.doorDown.SetActive(false);
            selectedRoom.doorUp.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.doorRight.SetActive(false);
            selectedRoom.doorLeft.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.doorLeft.SetActive(false);
            selectedRoom.doorRight.SetActive(false);
        }

        return true;
    }
    private void SpawnRandomRoom(Room[] roomList, HashSet<Vector2Int> vacantPlaces)
    {
        int roomNumber = Random.Range(0, _roomPresets.Length);
        Room NewRoom = Instantiate(roomList[roomNumber]);

        int limit = 500;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            NewRoom.RotateRandomly();

            if (ConnectToRoom(NewRoom, position))
            {
                Vector3 roomPosition = CalculateRoomPosition(NewRoom, position);
                NewRoom.transform.position = roomPosition;

                _placedRooms[position.x, position.y] = NewRoom;
                break;
            }
        }
    }

    private Vector3 CalculateRoomPosition(Room newRoom, Vector2Int position)
    {
        int xModifier = newRoom.GetRoomX();
        int yModifier = newRoom.GetRoomY();
        return new Vector3((position.x - Center(_levelX)) * xModifier , 0, (position.y - Center(_levelY) * yModifier));
    }

    private int Center(int x)
    {
        x = x / 2;
        return x;
    }
}
