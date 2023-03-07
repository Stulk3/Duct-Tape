using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnPointType _pointType;
    [SerializeField] private GameObject[] _objectSet;
    [SerializeField] private GameObject _generatedObject;
    private SpawnPoint _levelProgression;
    
    private void Start()
    {
        if (_pointType == SpawnPointType.Tile || _pointType == SpawnPointType.Room)
        {
            SpawnRandomObjectFromSet(_objectSet);
        }
    }
    public void SpawnExit()
    {
        _levelProgression.SpawnFirstObjectInSet();
    }
    public GameObject GetGeneratedObject()
    {
        return _generatedObject;
    }
    public void SpawnFirstObjectInSet()
    {
        _generatedObject = Instantiate(_objectSet[0], transform.position, Quaternion.identity);
        Debug.Log(_generatedObject);
    }
    public void DestroyItself()
    {
        Destroy(this.gameObject);
    }
    public void SpawnRandomObjectFromSet(GameObject[] objectSet)
    {
        int randomIndex = Random.Range(0, objectSet.Length);
        _generatedObject = Instantiate(objectSet[randomIndex], transform.position, Quaternion.identity);
        _generatedObject.transform.SetParent(this.transform);
        /// Более корректно будет спавнить их сразу на сцене в пуле, а потом брать из него, но я не успеваю накодить(((
    }
    public void SpawnObjectFromSet(int index)
    {
        _generatedObject = Instantiate(_objectSet[index], transform.position, Quaternion.identity);
       
    }
    public GameObject[] ObjectSet()
    {
        return _objectSet;
    }
    public enum SpawnPointType
    {
        Tile,Item,Enemy,InteractiveEntity, LevelProgression,Room
    }
}
