using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBox : MonoBehaviour
{
    public TMPro.TextMeshProUGUI characterNameText;
    public TMPro.TextMeshProUGUI dialogContentText;
    public bool autoPlayMode;
    private int currentLineIndex = 0;
    private List<DialogLine> dialogLines;
    public float textSpeed = 0.05f; // Tốc độ hiển thị từng ký tự
    //TODO: phát âm thanh khi hiển thị từng ký tự

    private Coroutine typingCoroutine;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right mouse button
        {
            ShowNextLine();
        }
    }

    // Phương thức để nhận đầu vào là một đối tượng DialogContent
    public void SetDialogContent(DialogContent content)
    {
        gameObject.SetActive(true);
        dialogLines = content.dialogLines;
        currentLineIndex = 0;
        ShowNextLine();
    }

    // Phương thức để hiển thị dòng hội thoại tiếp theo
    public void ShowNextLine()
    {
        if (currentLineIndex < dialogLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            characterNameText.text = dialogLines[currentLineIndex].actorStats.actorName;
            typingCoroutine = StartCoroutine(TypeSentence(dialogLines[currentLineIndex].dialogText));
            gameObject.SetActive(true);
            currentLineIndex++;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Phương thức để đóng hộp thoại
    public void CloseDialogBox()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    // Coroutine để hiển thị từng ký tự của nội dung hội thoại
    private IEnumerator TypeSentence(string sentence)
    {
        dialogContentText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogContentText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (autoPlayMode)
        {
            float waitTime = sentence.Length * textSpeed + 0.1f; // Thời gian chờ trước khi chuyển sang dòng tiếp theo
            yield return new WaitForSeconds(waitTime);
            ShowNextLine();
        }
    }
}