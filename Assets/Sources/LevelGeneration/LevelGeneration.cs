using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    

    [SerializeField] private Transform[] _startingPositions;
    [SerializeField] private GameObject[] _rooms;

    private int _direction;
    [SerializeField] private int _moveAmount;

    [SerializeField] private float _timeBetweenRoom;
    [SerializeField] private float _startTimeBetweenRoom = 0.25f;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;


    private bool stopGeneration;
    private void Start()
    {
        int randomIndex = Random.Range(0, _startingPositions.Length);
        transform.position = _startingPositions[randomIndex].position;
        Instantiate(_rooms[0], transform.position, Quaternion.identity);

        GenerateRandomDirectionValue(_direction);
    }
    private void Update()
    {
        if(_timeBetweenRoom <= 0 && !stopGeneration)
        {
            Move();
            _timeBetweenRoom = _startTimeBetweenRoom;
        }
        else
        {
            _timeBetweenRoom -= Time.deltaTime;
        }
    }
    private void Move()
    {
        if(_direction == 1 || _direction == 2) // Right
        {
            if (transform.position.x < maxX)
            {
                Vector2 newPosition = new Vector2(transform.position.x + _moveAmount, transform.position.y);
                transform.position = newPosition;
            }
            else
            {
                _direction = 5;
            }

        }
        else if (_direction == 3 || _direction == 4) // Left
        {
            if (transform.position.x > minX)
            {
                Vector2 newPosition = new Vector2(transform.position.x - _moveAmount, transform.position.y);
                transform.position = newPosition;
            }
            else
            {
                _direction = 5;
            }
        }
        else if (_direction == 5) // Down
        {
            if (transform.position.y > minY)
            {
                Vector2 newPosition = new Vector2(transform.position.x, transform.position.y - _moveAmount);
                transform.position = newPosition;
            }
            else
            {
                stopGeneration = true;
            }

        }

        Instantiate(_rooms[0], transform.position, Quaternion.identity);
        GenerateRandomDirectionValue(_direction);
        Debug.Log("Moved");
        Debug.Log(_direction);
    }
    private void GenerateRandomDirectionValue(int direction)
    {
        _direction = Random.Range(1, 6);
    }
}
