using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectSet;
    private GameObject _generatedObject;

    private void Start()
    {
        SpawnRandomObjectFromSet(_objectSet);
    }
    public void SpawnRandomObjectFromSet(GameObject[] objectSet)
    {
        int randomIndex = Random.Range(0, objectSet.Length);
        _generatedObject = Instantiate(objectSet[randomIndex], transform.position, Quaternion.identity);
        _generatedObject.transform.SetParent(this.transform);
        /// Более корректно будет спавнить их сразу на сцене в пуле, а потом брать из него, но я не успеваю накодить(((
    }
}
