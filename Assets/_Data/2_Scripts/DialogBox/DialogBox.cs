using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBox : MonoBehaviour
{
    public TMPro.TextMeshProUGUI characterNameText;
    public TMPro.TextMeshProUGUI dialogContentText;
    public bool autoPlayMode;
    public List<DialogActorPopupEffect> dialogActorPopupEffects;
    private int currentLineIndex = 0;
    private List<DialogLine> dialogLines;
    public float textSpeed = 0.05f; // Tốc độ hiển thị từng ký tự
    //TODO: phát âm thanh khi hiển thị từng ký tự

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right mouse button
        {
            if (isTyping)
            {
                CompleteCurrentLine();
            }
            else
            {
                ShowNextLine();
            }
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

            foreach (DialogActorPopupEffect dialogActorPopupEffect in dialogActorPopupEffects)
            {
                dialogActorPopupEffect.HideActorPopupAnimation();
                if (dialogActorPopupEffect.actorStats == dialogLines[currentLineIndex].actorStats)
                {
                    dialogActorPopupEffect.ShowActorPopupAnimation();
                }
            }
            characterNameText.text = dialogLines[currentLineIndex].actorStats.actorName;
            typingCoroutine = StartCoroutine(TypeSentence(dialogLines[currentLineIndex].dialogText));
            gameObject.SetActive(true);
            currentLineIndex++;
        }
        else // Kết thúc hội thoại
        {
            gameObject.SetActive(false);
        }
    }

    // Phương thức để hoàn thành dòng hiện tại ngay lập tức
    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogContentText.text = dialogLines[currentLineIndex - 1].dialogText;
        isTyping = false;
    }

    // Phương thức để bỏ qua hội thoại
    public void SkipDialog()
    {
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    // Coroutine để hiển thị từng ký tự của nội dung hội thoại
    private IEnumerator TypeSentence(string sentence)
    {
        dialogContentText.text = "";
        isTyping = true;
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogContentText.text += sentence[i];
            yield return new WaitForSeconds(textSpeed);

            if (autoPlayMode && i == sentence.Length - 1)
            {
                yield return new WaitForSeconds(0.5f);
                ShowNextLine();
            }
        }
        isTyping = false;
    }
}