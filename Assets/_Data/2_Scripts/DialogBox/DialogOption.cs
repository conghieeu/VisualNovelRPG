using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogOption
{
    public string optionText;
    [SerializeField] List<DialogContent> dialogContents;

    public void Select(DialogBox dialogBox)
    {
        Debug.Log($"Option selected");
    }
}