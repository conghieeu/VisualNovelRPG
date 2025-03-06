using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogBox : MonoBehaviour
{
    public TMPro.TextMeshProUGUI characterNameText;
    public TMPro.TextMeshProUGUI dialogContentText;
    public bool autoPlayMode;
    public List<DialogActorPopupEffect> dialogActorPopupEffects;
    public List<AudioClip> typingSounds; // Danh sách các âm thanh để phát khi hiển thị từng ký tự
    public float textSpeed = 0.05f; // Tốc độ hiển thị từng ký tự
    private int currentLineIndex = 0;
    private List<DialogLine> dialogLines;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private AudioSource audioSource;
    private DialogCtrl dialogCtrl;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dialogCtrl = GetComponentInParent<DialogCtrl>();
    }

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
    public void SetDialogLines(List<DialogLine> dialogLines)
    {
        gameObject.SetActive(true);
        this.dialogLines = dialogLines;
        currentLineIndex = 0;
        ShowNextLine();
    }

    #region In ra nội dung hội thoại

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
            if (dialogCtrl) dialogCtrl.SetCurrentLineIndex(currentLineIndex);
        }
        else // Kết thúc hội thoại
        {
            CloseDialog();
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
    public void CloseDialog()
    {
        dialogCtrl.dialogCollections.Clear();
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    // Coroutine để hiển thị từng ký tự của nội dung hội thoại
    private IEnumerator TypeSentence(string sentence)
    {
        dialogContentText.text = "";
        isTyping = true;
        for (int i = 0; i < sentence.Length; i++)
        {
            dialogContentText.text += sentence[i];

            // Phát âm thanh ngẫu nhiên khi hiển thị từng ký tự
            if (typingSounds.Count > 0)
            {
                AudioClip randomClip = typingSounds[Random.Range(0, typingSounds.Count)];
                audioSource.PlayOneShot(randomClip);
            }

            yield return new WaitForSeconds(textSpeed);

            if (autoPlayMode && i == sentence.Length - 1)
            {
                yield return new WaitForSeconds(0.5f);
                ShowNextLine();
            }
        }
        isTyping = false;

        // Kiểm tra nếu dòng hội thoại hiện tại có isEndDialog = true thì ngắt cuộc hội thoại
        if (dialogLines[currentLineIndex - 1].isEndDialog)
        {
            CloseDialog();
        }
    }

    #endregion
}