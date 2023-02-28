using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]
public class Action : MonoBehaviour
{
    public bool Hide = false;
    public ActionType Type;
    public enum ActionType : int
    {
        Dialogue = 1,
        Item = 2,
        VisualNovel = 3
    }

    [Space(height: 5f)]
    [Header("Trigger Collider Settings")]
    [SerializeField] private float TriggerColliderX = 3.2f;
    [SerializeField] private float TriggerColliderY = 2.5f;
    [SerializeField] private float TriggerColliderZ = 2.5f;

    [Space(height: 5f)]
    [Header("GameObjects")]

    [SerializeField] private GameObject Button;

    [SerializeField] public GameObject DialogueWindow;

    [Space(height: 5f)]
    [Header("Scripts")]

    public DialogueSystem.DialogueSystem DialogueSystem;

    public float StartDelay = 0;

    [SerializeField] bool DialogueWindowIsActive = false;
    [SerializeField] bool InActionRadius = false;
    [SerializeField] bool DialogueIsOver = false;


    private void Awake()
    {
        SetUpActionComponent();
    }
    private void Start()
    {
        if (Type == ActionType.VisualNovel)
        {
            StartCoroutine(StartDialogue(StartDelay));
        }
    }
    void Update()
    {

        UpdateBoolVariables();

        switch (Type)
        {
            case (ActionType.Dialogue):
                CheckForDialogueBeginning(); break;

            case (ActionType.Item): break;
        }


        ActionButtonDisappear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Есть в радиусе");
        if (collision.gameObject.tag == "Player" && InActionRadius == false && DialogueIsOver == false)
        {

            InActionRadius = true;
            if (Button != null)
            {
                Button.SetActive(true);
            }
        }
        /* switch (Type)
        {
            case (ActionType.Dialogue):
                DialogueOnTriggerEnter(collision); break;

            case (ActionType.Item): break;

            case (ActionType.VisualNovel): break;
        } */
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && InActionRadius == true && DialogueIsOver == false)
        {

            InActionRadius = false;
            if (Button != null)
            {
                Button.SetActive(false);
            }
        }
        /* switch (Type)
        {
            case (ActionType.Dialogue):
                DialogueOnTriggerExit(collision); break;

            case (ActionType.Item): break;

            case (ActionType.VisualNovel): break;
        } */
    }



    private void SetUpTriggerCollider()
    {
        BoxCollider2D TriggerCollider = this.gameObject.GetComponent<BoxCollider2D>();
        //TriggerCollider.isTrigger = true;
        Vector3 TriggerSize = new Vector3(TriggerColliderX, TriggerColliderY, TriggerColliderZ);
        TriggerCollider.size = TriggerSize;
    }



    void CheckForDialogueBeginning()
    {
        if (Input.GetKeyDown(KeyCode.E) && InActionRadius == true && DialogueWindowIsActive == false && DialogueIsOver == false)
        {
            OpenDialogueWindow();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && InActionRadius == true && DialogueWindowIsActive == false && DialogueIsOver == false)
        {
            OpenDialogueWindow();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && InActionRadius == true && DialogueWindowIsActive == false && DialogueIsOver == false)
        {
            OpenDialogueWindow();
        }
    }
    void StartOnlyDialogue()
    {
        StartCoroutine(StartDialogue(StartDelay));
    }

    void SetUpActionComponent()
    {
        /////// Диалог ///////
        if (Type == ActionType.Dialogue)
        {
            SetUpTriggerCollider();
            if (DialogueSystemComponentOnObject())
            {
                SetDialogueSystemComponent();
            }
        }

        /////// Предмет ///////
        else if (Type == ActionType.Item)
        {
            SetUpTriggerCollider();
        }


        /////// Диалог-Новелла ///////
        else if (Type == ActionType.VisualNovel)
        {
            if (DialogueSystemComponentOnObject())
            {
                SetDialogueSystemComponent();
            }
        }
    }


    private void ActionButtonDisappear()
    {
        if (DialogueWindowIsActive)
        {
            Button.SetActive(false);
        }
    }
    public void CloseDialogueWindow()
    {
        DialogueWindow.SetActive(false);
        DialogueWindowIsActive = false;
    }

    public void OpenDialogueWindow()
    {
        DialogueWindow.SetActive(true);
        DialogueWindowIsActive = true;
    }

    private void SetDialogueSystemComponent()
    {
        DialogueSystem = this.gameObject.GetComponent<DialogueSystem.DialogueSystem>();
    }

    private bool DialogueSystemComponentOnObject()
    {
        return this.GetComponent<DialogueSystem.DialogueSystem>() != null;
    }

    private void UpdateBoolVariables()
    {
        if (DialogueSystemComponentOnObject())
        {
            DialogueIsOver = DialogueSystem.GetDialogueIsOver();
        }
    }


    public bool GetInActionRadius()
    {
        return InActionRadius;
    }
    public bool GetDialogueWindowIsActive()
    {
        return DialogueWindowIsActive;
    }


    public IEnumerator StartDialogue(float StartDelay)
    {

        yield return new WaitForSeconds(StartDelay);

        OpenDialogueWindow();

    }
}
