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

    private StickerData stickerData;

    public void OpenSaveToFavoritesPopup()
    {
        popupPanel.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void CancelButtonOnClickListener()
    {
        this.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }

    public void SaveButtonOnClickListener()
    {
        
    }

    public void OnNameChangeActionListener()
    {
        //Check if name of the Saved Sticker already exists in the file. 
        if (stickerData.GetAllStickers().Exists(x => x.stickerName == saveName.text))
        {
            //Sticker Name already exists; display error.
            nameExistsError.gameObject.SetActive(true);
        }
        else if (nameExistsError.IsActive())
        {
            nameExistsError.gameObject.SetActive(false);
        }
    }
}
