using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Mesh[] _blockMeshes;
    public GameObject doorUp;
    public GameObject doorDown;
    public GameObject doorRight;
    public GameObject doorLeft;

    [Header("Size of Room")]
    [SerializeField] private int _roomX;
    [SerializeField] private int _roomY;
    private void Awake()
    {
        
    }

    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);
            var temporaryDoorLeft = doorLeft;

            doorLeft = doorDown;
            doorDown = doorRight;
            doorRight = doorUp;
            doorUp = temporaryDoorLeft;
        }
    }

    public int GetRoomX()
    {
        return _roomX;
    }
    public int GetRoomY()
    {
        return _roomY;
    }
}
