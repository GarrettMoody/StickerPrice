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
    public TextMeshProUGUI saveName;
    public Button saveButton;
    public Button cancelButton;
    public GameObject popupPanel;


    public void OpenSaveToFavoritesPopup()
    {
        popupPanel.SetActive(true);
        this.gameObject.SetActive(true);

    }

}
