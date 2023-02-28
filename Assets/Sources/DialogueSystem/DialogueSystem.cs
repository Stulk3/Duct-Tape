using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DialogueSystem
{

    [RequireComponent(typeof(Action))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(DialogueVertexAnimator))]
    public class DialogueSystem : MonoBehaviour
    {
        [Header("Text Settings")]
        [SerializeField] TextMeshProUGUI NameComponent;
        [SerializeField] TextMeshProUGUI TextComponent;

        [Space(5f)]
        [Header("Starting Dialogue")]
        [SerializeField] DialogueLine dialogue;

        [Space(5f)]
        [Header("Sound Settings")]
        [SerializeField] AudioClip SoundEffect;

        [SerializeField] AudioClip CharacterVoice;

        [Header("Components")]
        public AudioSource DialogueVoiceSource;
        [SerializeField] DialogueVertexAnimator DialogueVertexAnimator;
        public Action ActionComponent;
        public AudioSourceGroup AudioSourceGroup;


        private bool DialogueIsOver = false;

        private bool InActionRadius;
        private bool DialogueWindowIsActive;
        private bool DialogueIsPlayed = false;

        void Awake()
        {
            DialogueVertexAnimator = new DialogueVertexAnimator(TextComponent, AudioSourceGroup);
        }

        void Start()
        {

            if (DialogueVoiceSource == null)
            {
                DialogueVoiceSource = this.GetComponent<AudioSource>();
            }

            if (ActionComponent == null)
            {
                ActionComponent = this.GetComponent<Action>();
            }

            if (DialogueVertexAnimator == null)
            {
                DialogueVertexAnimator = this.GetComponent<DialogueVertexAnimator>();
            }

        }
        void Update()
        {
            UpdateBoolVariables();
            ManageDialogue();
        }
        private void ManageDialogue()
        {
            ManageDialogueLineChange();

            ManageCharacterVoice();

            ManageDialogueWindowContent();
        }

        private Coroutine typeRoutine = null;
        void PlayDialogue(string message)
        {
            DialogueIsPlayed = true;
            this.EnsureCoroutineStopped(ref typeRoutine);
            DialogueVertexAnimator.textAnimating = false;
            List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
            typeRoutine = StartCoroutine(DialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, CharacterVoice, null));
        }

        string SetDialogueName(string name)
        {
            List<DialogueCommand> commands = DialogueUtility.ProcessInputString(name, out string FinalName);
            return FinalName;
        }

        private void ManageDialogueLineChange()
        {
            DialogueLine nextDialogueLine = dialogue.GetNextDialogue();
            if (InputForDialogueChangeIsPressed(nextDialogueLine))
            {
                dialogue = nextDialogueLine;
                StopVoiceAudioPlaying();
                DialogueIsPlayed = false;
            }
            if (InputForDialogueCloseIsPressed(nextDialogueLine))
            {
                DialogueIsOver = true;
                StopVoiceAudioPlaying();
                ActionComponent.CloseDialogueWindow();
            }



        }
        void ManageDialogueWindowContent()
        {
            NameComponent.text = dialogue.GetDialogueName();
            CharacterVoice = dialogue.GetCharacterVoice();
            if (DialogueWindowIsActive && !DialogueIsPlayed)
            {
                NameComponent.text = SetDialogueName(dialogue.GetDialogueName());
                PlayDialogue(dialogue.GetDialogueText());
            }
        }
        void ManageCharacterVoice()
        {
            if (Input.GetKeyDown(KeyCode.E) && CharacterVoice != null && !DialogueIsOver && InActionRadius)
            {
                DialogueVoiceSource.clip = CharacterVoice;
                DialogueVoiceSource.Play();
            }

            else if (Input.GetKeyDown(KeyCode.Mouse0) && CharacterVoice != null && !DialogueIsOver && InActionRadius)
            {
                DialogueVoiceSource.clip = CharacterVoice;
                DialogueVoiceSource.Play();
            }

            else if (Input.GetKeyDown(KeyCode.Space) && CharacterVoice != null && !DialogueIsOver && InActionRadius)
            {
                DialogueVoiceSource.clip = CharacterVoice;
                DialogueVoiceSource.Play();
            }
        }
        bool InputForDialogueChangeIsPressed(DialogueLine nextDialogueLine)
        {
            if (Input.GetKeyDown(KeyCode.E) && DialogueWindowIsActive && nextDialogueLine != null && !DialogueVertexAnimator.textAnimating)
            {
                return true;
            }

            else if (Input.GetKeyDown(KeyCode.Mouse0) && DialogueWindowIsActive && nextDialogueLine != null && !DialogueVertexAnimator.textAnimating)
            {
                return true;
            }

            else if (Input.GetKeyDown(KeyCode.Space) && DialogueWindowIsActive && nextDialogueLine != null && !DialogueVertexAnimator.textAnimating)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool InputForDialogueCloseIsPressed(DialogueLine nextDialogueLine)
        {
            if (nextDialogueLine == null && DialogueWindowIsActive && (Input.GetKeyDown(KeyCode.E)))
            {
                return true;
            }

            else if (nextDialogueLine == null && DialogueWindowIsActive && (Input.GetKeyDown(KeyCode.Mouse0)))
            {
                return true;
            }

            else if (nextDialogueLine == null && DialogueWindowIsActive && (Input.GetKeyDown(KeyCode.Space)))
            {
                return true;
            }

            else
            {
                return false;
            }

        }
        void UpdateBoolVariables()
        {
            DialogueWindowIsActive = ActionComponent.GetDialogueWindowIsActive();
            InActionRadius = ActionComponent.GetInActionRadius();
        }
        void StopVoiceAudioPlaying()
        {
            DialogueVoiceSource.Stop();
        }

        public bool GetDialogueIsOver()
        {
            return DialogueIsOver;
        }
    }
}
