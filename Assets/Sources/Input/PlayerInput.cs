using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayableCharacter))]
public class PlayerInput : MonoBehaviour
{
    private Controll _controll;
    [SerializeField] private PlayableCharacter _character;
    private List<IInteractable> _interactableEntity = new List<IInteractable>();
    private List<ITakeFix> _fixableEntity = new List<ITakeFix>(); 

    private void Awake()
    {
        _controll = new Controll();
        SubscribeToInputSystem(_controll);
        if (_character == null)
        {
            _character = GetComponent<PlayableCharacter>();
        }
    }
    private void Move(PlayableCharacter playableCharacter)
    {
        Vector2 moveInput = _controll.Player.Move.ReadValue<Vector2>();

        playableCharacter.Move(moveInput);
    }
    private void Look()
    {
        Vector2 mousePosition = _controll.Player.Look.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 rotation = mouseWorldPosition - transform.position;
        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        _character.upperBody.transform.rotation = Quaternion.Euler(0,0,rotationZ);

    }
    private void Stop(PlayableCharacter playableCharacter)
    {
        playableCharacter.characterRigidbody.velocity = new Vector2(0,0);
    }
    private void Interact(IInteractable interactableEntity)
    {
        if(interactableEntity != null)
        {
            interactableEntity.Interact();
        }
    }
    private void Fix(ITakeFix fixableEntity)
    {
        if(fixableEntity != null && _character.GetTapeCount() >= 1)
        {
            fixableEntity.TakeFix(1);
        }
    }
    private void SubscribeToInputSystem(Controll controll)
    {
        controll.Player.Move.performed += context => Move(_character);
        controll.Player.Move.canceled += context => Stop(_character);
        controll.Player.Look.performed += context => Look();
        controll.Player.Interact.performed += context => Interact(_interactableEntity[0]);
        controll.Player.Fix.performed += context => Fix(_fixableEntity[0]);
    }

    private void OnEnable()
    {
        _controll.Enable();
    }
    private void OnDisable()
    {
        _controll.Disable();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        _interactableEntity.Add(collider.gameObject.GetComponent<IInteractable>());
        _fixableEntity.Add(collider.gameObject.GetComponent<ITakeFix>());
  
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        _interactableEntity.Remove(collider.gameObject.GetComponent<IInteractable>());
        _fixableEntity.Remove(collider.gameObject.GetComponent<ITakeFix>());
    }
}
