using UnityEngine;
using UnityEngine.UI;

public class btnOpenDialog : MonoBehaviour
{
    Button thisButton;
    public DialogBox dialogBox;

    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OpenDialog);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenDialog();
        }
    }

    void OpenDialog()
    {
        if (dialogBox.gameObject.activeSelf) return;

        PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            NPC npcInteract = playerInteraction.GetNPCInteract();
            if (npcInteract != null)
            {
                dialogBox.SetDialogContent(npcInteract.dialogContent);
            }
        }
    }
}