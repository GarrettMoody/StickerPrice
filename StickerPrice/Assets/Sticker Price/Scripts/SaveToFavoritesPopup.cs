using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveToFavoritesPopup : MonoBehaviour
{
    public Toggle stickerToggle;
    public Toggle templateToggle;
    public ToggleGroup saveType;
    public InputField saveName;
    public Button saveButton;
    public GameObject popupPanel;

    private StickerData stickerData = new StickerData();

    public void OpenSaveToFavoritesPopup()
    {
        popupPanel.SetActive(true);
        this.gameObject.SetActive(true);
        stickerData.ReadStickers();
    }

    public void CancelButtonOnClickListener()
    {
        this.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }

    public void SaveNameOnChangeListener()
    {
        //Check if name of the Saved Sticker already exists in the file. 
        if (stickerData.GetStickers().Exists(x => x.stickerName == saveName.text))
        {
            //Sticker Name already exists; display error.

        }
    }

}
