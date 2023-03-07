using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelGeneration
{
    public class LevelGeneratingDatabase : MonoBehaviour
    {
        public static LevelGeneratingDatabase instance;

        [SerializeField] private GameObject[] _fillerRooms;
        [SerializeField] private GameObject[] _LeftToRightRooms;
        [SerializeField] private GameObject[] _LeftRightUpRooms;
        [SerializeField] private GameObject[] _LeftRightDownRooms;
        [SerializeField] private GameObject[] _LeftRighUpDownRooms;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        public static GameObject[] GetFillerRooms() => instance._fillerRooms;
        public static GameObject[] GetLeftToRightRooms() => instance._LeftToRightRooms;
        public static GameObject[] GetLeftRightUpRooms() => instance._LeftRightUpRooms;
        public static GameObject[] GetLeftRightDownRooms() => instance._LeftRightDownRooms;
        public static GameObject[] GetLeftRighUpDownRooms() => instance._LeftRighUpDownRooms;

    }
}

