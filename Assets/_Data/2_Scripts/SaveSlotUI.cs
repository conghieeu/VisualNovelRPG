using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private Button slotButton;
    [SerializeField] private TextMeshProUGUI slotNumberText;  // Luôn hiển thị
    [SerializeField] private TextMeshProUGUI timeText;        // Trong DataContainer
    [SerializeField] private TextMeshProUGUI chapterNameText;
    [SerializeField] private RawImage screenshotImage;
    [SerializeField] private GameObject noDataText;
    [SerializeField] private GameObject dataContainer;

    private int slotIndex;
    private Action<int> onSlotSelected;

    public void Initialize(int index, Action<int> callback)
    {
        slotIndex = index;
        onSlotSelected = callback;
        slotButton.onClick.AddListener(OnSlotClicked);
        slotNumberText.text = $"{index + 1}.";  // Số slot luôn hiển thị
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "saves", $"save_{slotIndex}.json");
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            
            dataContainer.SetActive(true);
            noDataText.SetActive(false);
            
            timeText.text = saveData.saveTime.ToString("MMM dd, HH:mm");
            chapterNameText.text = saveData.chapterName;

            if (!string.IsNullOrEmpty(saveData.screenshotBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(saveData.screenshotBase64);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(imageBytes);
                screenshotImage.texture = tex;
            }
        }
        else
        {
            dataContainer.SetActive(false);
            noDataText.SetActive(true);
            noDataText.GetComponent<TextMeshProUGUI>().text = "NO DATA";
        }
    }

    private void OnSlotClicked()
    {
        onSlotSelected?.Invoke(slotIndex);
    }
} 