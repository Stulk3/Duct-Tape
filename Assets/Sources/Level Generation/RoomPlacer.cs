using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public Room StartingRoom;

    private Room[,] spawnedRooms;

    private IEnumerator Start()
    {
        spawnedRooms = new Room[11, 11];
        spawnedRooms[5, 5] = StartingRoom;

        for (int i = 0; i < 12; i++)
        {
            // Ёто вот просто убрать чтобы подземелье генерировалось мгновенно на старте
            yield return new WaitForSecondsRealtime(0.5f);

            PlaceOneRoom();
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        // Ёту строчку можно заменить на выбор комнаты с учЄтом еЄ веро€тности, вроде как в ChunksPlacer.GetRandomChunk()
        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);

        int limit = 500;
        while (limit-- > 0)
        {
            // Ёту строчку можно заменить на выбор положени€ комнаты с учЄтом того насколько он далеко/близко от центра,
            // или сколько у него соседей, чтобы генерировать более плотные, или наоборот, раст€нутые данжи
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            newRoom.RotateRandomly();

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 12;
                spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
        }

        Destroy(newRoom.gameObject);
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.doorUp != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.doorDown != null) neighbours.Add(Vector2Int.up);
        if (room.doorDown != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.doorUp != null) neighbours.Add(Vector2Int.down);
        if (room.doorRight != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.doorLeft != null) neighbours.Add(Vector2Int.right);
        if (room.doorLeft != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.doorRight != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

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
}