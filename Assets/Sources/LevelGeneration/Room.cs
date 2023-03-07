using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _itemSpawnPoints;
    [SerializeField] private int _type;
    [SerializeField] private SpawnPoint _levelProgressionPoint;
    public int GetRoomType()
    {
        return _type;
    }
    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }
    public void SpawnItemInRandomPoint(SpawnPoint spawnPoint)
    {
        spawnPoint.SpawnRandomObjectFromSet(spawnPoint.ObjectSet());
    }
    public void SpawnLevelProgression()
    {
        _levelProgressionPoint.SpawnFirstObjectInSet(); ;
    }
    public SpawnPoint[] GetItemSpawnPoints()
    {
        return _itemSpawnPoints;
    }
    
}
