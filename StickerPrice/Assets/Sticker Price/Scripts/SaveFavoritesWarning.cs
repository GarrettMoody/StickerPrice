using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFavoritesWarning : MonoBehaviour
{
    public GameObject popupPanel;
    public EditStickerPanel editStickerPanel;

    public void OpenSaveFavoritesWarning()
    {
        popupPanel.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void OnCancelButtonClick()
    {
        this.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }

    public void OnOKButtonClick()
    {
        editStickerPanel.CloseEditStickerPanel(true);
        this.gameObject.SetActive(false);
        popupPanel.SetActive(false);
    }
}
