using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private Controll _controll;
    [SerializeField] private Character _character;
    [SerializeField] private Rigidbody2D characterRigidbody;
    [SerializeField] private GameObject upperBody;
    [SerializeField] private GameObject lowerBody;
    [SerializeField] private float _speed;
    private void Awake()
    {
        _controll = new Controll();
        SubscribeToInputSystem(_controll);
        if (characterRigidbody == null)
        {
            characterRigidbody = GetComponent<Rigidbody2D>();
            //  characterRigidbody.Ge
        }
    }
    private void Update()
    {
    }
    private void Move()
    {
        Vector2 moveInput = _controll.Player.Move.ReadValue<Vector2>();

        characterRigidbody.velocity = moveInput * _speed;
    }
    private void Look()
    {
        Vector2 mousePosition = _controll.Player.Look.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 rotation = mouseWorldPosition - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        upperBody.transform.rotation = Quaternion.Euler(0,0,rotationZ);

    }
    private void Stop()
    {
        characterRigidbody.velocity = new Vector2(0,0);
    }
    private void SubscribeToInputSystem(Controll controll)
    {
        controll.Player.Move.performed += context => Move();
        controll.Player.Move.canceled += context => Stop();
        controll.Player.Look.performed += context => Look();
    }

    private void OnEnable()
    {
        _controll.Enable();
    }
    private void OnDisable()
    {
        _controll.Disable();
    }
}
