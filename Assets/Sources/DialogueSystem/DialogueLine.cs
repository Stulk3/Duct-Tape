using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="Dialogue/Dialogue Line")]
public class DialogueLine : ScriptableObject
{
    [TextArea(1, 1)] [SerializeField] string CharacterName;
    [TextArea(10, 14)] [SerializeField] string DialogueText;
    [SerializeField] AudioClip SoundEffect;
    [SerializeField] AudioClip CharacterVoice;
    [SerializeField] DialogueLine nextDialogue;

    private int index = 0;

    public string GetDialogueName()
        {
            return CharacterName;
        }
    public string GetDialogueText()
    {
        return DialogueText;
    }
    public DialogueLine GetNextDialogue()
    {
        return nextDialogue;
    }
    public void GetNextLine()
    {
        index++;
    }
    public AudioClip GetCharacterVoice()
    {
        return CharacterVoice;
    }

    public AudioClip GetSoundEffect()
    {
        return SoundEffect;
    }

}
