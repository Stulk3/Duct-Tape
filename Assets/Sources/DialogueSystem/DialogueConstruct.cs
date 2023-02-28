using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;

[CreateAssetMenu(menuName ="Dialogue/Dialogue")]

public class DialogueConstruct : ScriptableObject
{

   [SerializeField] string FolderName;
   [SerializeField] List<DialogueLine> Lines;
   [SerializeField] int LineIndex;
   private string FolderIndex;
   private string assetPath;
   private DialogueLine dialogueLine;

    public void ConstuctDialogue()
    {

    }
    public void AddLineToArray()
    {
        Lines[LineIndex] = dialogueLine;
    }
    public void CheckLinesIndex()
    {
        //if (AssetDatabase.FindAssets(FolderIndex + LineIndex   +".asset"))
        {

        }
    }
    public void SetLineIndexToZero()
    {
        LineIndex = 0;
    }
    public void RefreshData()
    {

    }
    public void SetFolderIndex()
    {
        FolderIndex = FolderName.Substring(0,3);
    }
    public void CreateDialogueLine()
    {
        CreateFolder();
        SetFolderIndex();
        dialogueLine = ScriptableObject.CreateInstance<DialogueLine>();
        AssetDatabase.CreateAsset(dialogueLine, assetPath + "/"+ FolderIndex + "#" + LineIndex   +".asset");
        AddLineToArray();
        LineIndex++;
        //ProjectWindowUtil позволяет при создании сразу открывать Ассет
    }

    public void CreateFolder()
    {
        assetPath = "Assets/Dialogues/" + FolderName;
        if(!Directory.Exists(assetPath))
        Directory.CreateDirectory(assetPath);
        AssetDatabase.Refresh();
    }

}




[CustomEditor(typeof(DialogueConstruct))]
public class DialogueConstructEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        DialogueConstruct construct = (DialogueConstruct)target;

        if(GUILayout.Button("Create Dialogue Line"))
        {
            construct.CreateDialogueLine();
        }
        if(GUILayout.Button("Set LineIndex to 0"))
        {
            construct.SetLineIndexToZero();
        }
    }
}