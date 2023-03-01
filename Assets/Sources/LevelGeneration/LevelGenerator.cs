using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator instance;

        [SerializeField] private Transform[] _startingPositions;
        [SerializeField] private RoomPosition[] _roomPositions;
        [SerializeField] private GameObject[] _roomSets;
        private Transform _entranceTransform;

        [SerializeField] private List<GameObject> _spawnedRooms;

        private Direction _direction;
        private int _directionIndex;

        private bool _stopGeneration;
        private int _downCounter;

        public float _moveIncrement;
        private float _spawnCooldown;
        public float startTimeBtwSpawn;

        public LayerMask _roomLayerMask;

        [Range(0, 1f)]
        private float _changeUp;

        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        private void Awake()
        {
            if (instance == null) instance = this;
            SpawnInitialRoom(_startingPositions);

            _directionIndex = Random.Range(1, 6);
        }

        private void Update()
        {

            if (_spawnCooldown <= 0 && _stopGeneration == false)
            {
                GenerateLevel();
                _spawnCooldown = startTimeBtwSpawn;
            }
            else
            {
                _spawnCooldown -= Time.deltaTime;
            }
        }

        private void GenerateLevel()
        {

            if (_directionIndex == 1 || _directionIndex == 2)
            {

                if (transform.position.x < maxX)
                {
                    _downCounter = 0;
                    Vector2 pos = new Vector2(transform.position.x + _moveIncrement, transform.position.y);
                    transform.position = pos;

                    int randomRoomIndex = Random.Range(1, 4);
                    SpawnRoom(_roomSets[randomRoomIndex]);


                    _directionIndex = Random.Range(1, 6);
                    if (_directionIndex == 3)
                    {
                        _directionIndex = 1;
                    }
                    else if (_directionIndex == 4)
                    {
                        _directionIndex = 5;
                    }
                }
                else
                {
                    _directionIndex = 5;
                }
            }
            else if (_directionIndex == 3 || _directionIndex == 4)
            {

                if (transform.position.x > minX)
                {
                    _downCounter = 0;
                    Vector2 pos = new Vector2(transform.position.x - _moveIncrement, transform.position.y);
                    transform.position = pos;

                    int randomRoomIndex = Random.Range(1, 4);
                    Instantiate(_roomSets[randomRoomIndex], transform.position, Quaternion.identity);

                    _directionIndex = Random.Range(3, 6);
                }
                else
                {
                    _directionIndex = 5;
                }

            }
            else if (_directionIndex == 5)
            {
                _downCounter++;
                if (transform.position.y > minY)
                {

                    Collider2D previousRoomCollider = Physics2D.OverlapCircle(transform.position, 1, _roomLayerMask);
                    Debug.Log(previousRoomCollider);
                    Room previousRoom = previousRoomCollider.GetComponent<Room>();
                    int previousRoomType = previousRoom.GetRoomType();
                    if (previousRoomType != 4 && previousRoomType != 2)
                    {



                        if (_downCounter >= 2)
                        {
                            previousRoom.DestroyItself();
                            SpawnRoom(_roomSets[4]);
                        }
                        else
                        {
                            previousRoom.DestroyItself();
                            int randRoomDownOpening = Random.Range(2, 5);
                            if (randRoomDownOpening == 3)
                            {
                                randRoomDownOpening = 2;
                            }
                            SpawnRoom(_roomSets[randRoomDownOpening]);
                        }

                    }



                    Vector2 pos = new Vector2(transform.position.x, transform.position.y - _moveIncrement);
                    transform.position = pos;

                    int randomRoomIndex = Random.Range(3, 5);
                    Instantiate(_roomSets[randomRoomIndex], transform.position, Quaternion.identity);

                    _directionIndex = Random.Range(1, 6);
                }
                else
                {
                    _stopGeneration = true;
                    SpawnEntrance(_spawnedRooms);
                    SpawnExit(_spawnedRooms);

                    FillEmptyRoomPositions(_roomPositions);
                }

            }
        }
        private void SpawnInitialRoom(Transform[] startingPositions)
        {
            int randomStartingPosition = Random.Range(0, _startingPositions.Length);
            transform.position = startingPositions[randomStartingPosition].position;
            _entranceTransform = this.transform;
            SpawnRoom(_roomSets[4]);
        }
        private void SpawnRoom(GameObject room)
        {
            Instantiate(room, transform.position, Quaternion.identity);
            _spawnedRooms.Add(room);
        }
        private void SpawnEntrance(List<GameObject> rooms)
        {
            SpawnPoint firstRoom = rooms[0].GetComponent<SpawnPoint>();

        }
        private void SpawnExit(List<GameObject> rooms)
        {
            SpawnPoint lastRoom = rooms.Last().GetComponent<SpawnPoint>();

        }
        private void FillEmptyRoomPositions(RoomPosition[] roomPositions)
        {
            foreach (RoomPosition roomPosition in roomPositions)
            {
                roomPosition.FillEmptyRoomPositions();
            }
        }
        public void AddRoomToSpawnedRooms(GameObject room)
        {
            _spawnedRooms.Add(room);
        }
        public enum Direction
        {
            Right = 0,
            Left = 1,
            Up = 2,
            Down = 3
        }
    }

}
