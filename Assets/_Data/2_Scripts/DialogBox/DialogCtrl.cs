using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogCtrl : Singleton<DialogCtrl>
{
    public DialogBox dialogBox;
    public int currentDialogCollectionIndex;
    public List<DialogCollection> dialogCollections;

    public void Start()
    {

    }

    public void AddDialogCollection(DialogContent dialogContent)
    {
        DialogCollection dialogCollection = new DialogCollection();
        dialogCollection.dialogLines = dialogContent.dialogLines;
        this.dialogCollections.Add(dialogCollection);
    }

    public void GetCurrentLineIndex()
    {
        currentDialogCollectionIndex = dialogCollections[currentDialogCollectionIndex].currentLineIndex;
    }

    public void SetCurrentLineIndex(int index)
    {
        dialogCollections[currentDialogCollectionIndex].currentLineIndex = index;
    }

    public void SetDialogBoxLines()
    {
        dialogBox.SetDialogLines(dialogCollections[currentDialogCollectionIndex].dialogLines);
    }

    public void OpenDialogBox(DialogContent dialogContent)
    {
        if (dialogBox.gameObject.activeSelf) return;

        AddDialogCollection(dialogContent);
        SetDialogBoxLines();
    }

    [Serializable]
    public class DialogCollection
    {
        public int currentLineIndex;
        public List<DialogLine> dialogLines;
    }
}
