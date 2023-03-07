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
        [SerializeField] private int _itemsCount;
        private Transform _entranceTransform;

        private GameObject[] _allRooms;
        private GameObject[] _LeftToRightRooms;
        private GameObject[] _LeftRightUpRooms;
        private GameObject[] _LeftRightDownRooms;
        private GameObject[] _LeftRighUpDownRooms;
        private GameObject[] _fillerRooms;

        [SerializeField] private GameObject _player;

        [SerializeField] private List<GameObject> _spawnedRooms;
        [SerializeField] private List<SpawnPoint> _itemsSpawnPoints = new List<SpawnPoint>();

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

        }
        private void Start()
        {
            _LeftToRightRooms = LevelGeneratingDatabase.GetLeftToRightRooms();
            _LeftRightUpRooms = LevelGeneratingDatabase.GetLeftRightUpRooms();
            _LeftRightDownRooms = LevelGeneratingDatabase.GetLeftRightDownRooms();
            _LeftRighUpDownRooms = LevelGeneratingDatabase.GetLeftRighUpDownRooms();
            _fillerRooms = LevelGeneratingDatabase.GetFillerRooms();
            _directionIndex = Random.Range(1, 6);
            SpawnInitialRoom(_startingPositions);
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

                    //int randomRoomIndex = Random.Range(1, 4);
                    //SpawnRoom(_roomSets[randomRoomIndex]);
                    SpawnRandomRoomFromSet(_LeftRightUpRooms, _LeftRightDownRooms, _LeftRighUpDownRooms);

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

                    //int randomRoomIndex = Random.Range(1, 4);
                    //SpawnRoom(_roomSets[randomRoomIndex]);
                    SpawnRandomRoomFromSet(_LeftRightUpRooms, _LeftRightDownRooms, _LeftRighUpDownRooms);

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
                            DestroyRoom(previousRoom);
                            //previousRoom.DestroyItself();
                            SpawnRandomRoomFromSet(_LeftRighUpDownRooms);
                        }
                        else
                        {
                            DestroyRoom(previousRoom);
                            //previousRoom.DestroyItself();
                            //int randRoomDownOpening = Random.Range(2, 5);
                            //if (randRoomDownOpening == 3)
                            //{
                            //    randRoomDownOpening = 2;
                            //}
                            SpawnRandomRoomFromSet(_LeftRightDownRooms, _LeftRighUpDownRooms);
                            //SpawnRoom(_roomSets[randRoomDownOpening]);
                        }

                    }



                    Vector2 pos = new Vector2(transform.position.x, transform.position.y - _moveIncrement);
                    transform.position = pos;

                    //int randomRoomIndex = Random.Range(3, 5);
                    //Instantiate(_roomSets[randomRoomIndex], transform.position, Quaternion.identity);

                    SpawnRandomRoomFromSet(_LeftRightDownRooms, _LeftRighUpDownRooms);

                    _directionIndex = Random.Range(1, 6);
                }
                else
                {
                    _stopGeneration = true;
                    SpawnExit(_spawnedRooms);
                    FillEmptyRoomPositions(_roomPositions);
                    SpawnItems(_itemsCount);
                }

            }
        }
        private void SpawnInitialRoom(Transform[] startingPositions)
        {
            int randomStartingPosition = Random.Range(0, _startingPositions.Length);
            transform.position = startingPositions[randomStartingPosition].position;
            _entranceTransform = this.transform;
            _player.transform.position = _entranceTransform.position;
            
            //SpawnRoom(initialRoomSet[4]);
            SpawnRandomRoomFromSet(_LeftRighUpDownRooms);
        }
        private void SpawnRoom(GameObject room)
        {
            Instantiate(room, transform.position, Quaternion.identity);
            _spawnedRooms.Add(room);
        }
        private void SpawnRandomRoomFromSet(GameObject[] set)
        {
            int randomIndex = Random.Range(0, set.Length);
            GameObject spawnedRoom = Instantiate(set[randomIndex], transform.position, Quaternion.identity);
            _spawnedRooms.Add(spawnedRoom);
        }
        private void SpawnRandomRoomFromSet(GameObject[] set1, GameObject[] set2)
        {
            GameObject[] overallSet = set1.Concat(set2).ToArray();
            int randomIndex = Random.Range(0, overallSet.Length);
            GameObject spawnedRoom = Instantiate(overallSet[randomIndex], transform.position, Quaternion.identity);
            _spawnedRooms.Add(spawnedRoom);
        }
        private void SpawnRandomRoomFromSet(GameObject[] set1, GameObject[] set2, GameObject[] set3)
        {
            GameObject[] overallSet = set1.Concat(set2).Concat(set3).ToArray();
            int randomIndex = Random.Range(0, overallSet.Length);
            GameObject spawnedRoom = Instantiate(overallSet[randomIndex], transform.position, Quaternion.identity);
            _spawnedRooms.Add(spawnedRoom);
        }
        private void DestroyRoom(Room room)
        {
            room.DestroyItself();
            _spawnedRooms.Remove(room.gameObject);
        }
        private void SpawnRandomRoom()
        {
            int randomType = Random.Range(0, 4);
            switch (randomType)
            {
                case 0:
                    SpawnRandomRoomFromSet(_LeftToRightRooms);
                break;

                case 1:
                    SpawnRandomRoomFromSet(_LeftRightUpRooms);
                break;

                case 2:
                    SpawnRandomRoomFromSet(_LeftRightDownRooms);
                break;

                case 3:
                    SpawnRandomRoomFromSet(_LeftRighUpDownRooms);
                break;
            }
        }
        private void SpawnExit(List<GameObject> rooms)
        {
            Room lastRoom = rooms.Last().GetComponent<Room>();
            lastRoom.GetComponent<Room>().SpawnLevelProgression();

        }
        private void SpawnItems(int count)
        {
            
            List<Room> rooms = new List<Room>();
            for (int i = 0; i < _spawnedRooms.Count; i++)
            {
                if (_spawnedRooms[i].TryGetComponent<Room>(out Room room))
                {
                    rooms.Add(room);
                }
            }
            foreach(Room room in rooms)
            {
                 _itemsSpawnPoints.AddRange(room.GetItemSpawnPoints());
            }

            for (int i = 0; i < _itemsCount; i++)
            {
                int randomIndex = Random.Range(0, _itemsSpawnPoints.Count);
                _itemsSpawnPoints[randomIndex].SpawnFirstObjectInSet();
                _itemsSpawnPoints.RemoveAt(randomIndex);
            }

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
