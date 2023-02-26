using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles;
    private GameObject _generatedTile;

    private void Start()
    {
        SpawnRandomTileFromSet(_tiles);
    }
    private void SpawnRandomTileFromSet(GameObject[] tileSet)
    {
        int randomIndex = Random.Range(0, tileSet.Length);
        _generatedTile = Instantiate(tileSet[randomIndex], transform.position, Quaternion.identity);
        _generatedTile.transform.SetParent(this.transform);
        /// Более корректно будет спавнить их сразу на сцене в пуле, а потом брать из него, но я не успеваю накодить(((
    }
}
