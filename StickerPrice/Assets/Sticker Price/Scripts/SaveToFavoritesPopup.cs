using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveToFavoritesPopup : MonoBehaviour
{
    public Toggle stickerToggle;
    public Toggle templateToggle;
    public ToggleGroup saveType;
    public InputField saveName;
    public Button saveButton;
    public Button cancelButton;
    public GameObject popupPanel;
    public TextMeshProUGUI nameExistsError;
    public StickerDetailMenu stickerDetailMenu;

    private Sticker sticker;
    private StickerData stickerData = new StickerData();

    public void OpenSaveToFavoritesPopup(Sticker sticker)
    {
        this.sticker = sticker;
        stickerData = new StickerData();
        popupPanel.SetActive(true);
        this.gameObject.SetActive(true);
        OnNameChangeActionListener();
    }

    public void CancelButtonOnClickListener()
    {
        CloseSaveToFavoritesPopup();
    }

    public void SaveButtonOnClickListener()
    {
        sticker.stickerName = saveName.text;
        stickerData.AddSticker(sticker);
        CloseSaveToFavoritesPopup();
        stickerDetailMenu.stickerSavedPopup.DisplayPopup(2);
    }

    public void OnNameChangeActionListener()
    {
        //Check if name of the Saved Sticker already exists in the file. 
        if (stickerData.GetAllStickers().Exists(x => x.stickerName == saveName.text))
        {
            //Sticker Name already exists; display error.
            nameExistsError.gameObject.SetActive(true);
            saveButton.interactable = false;
        }
        else if (nameExistsError.IsActive())
        {
            nameExistsError.gameObject.SetActive(false);
            saveButton.interactable = true;
        }
    }

    private void CloseSaveToFavoritesPopup()
    {
        this.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }
}
