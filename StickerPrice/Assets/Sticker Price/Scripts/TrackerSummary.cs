using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerSummary : MonoBehaviour
{
    public ToggleGroup sortType;
    public Toggle dateToggle;
    public Toggle ownerToggle;
    public TransactionByDateList transactionByDateList;
    public TransactionByDateRow transactionByDateRowPrefab;
    public StickerDetailMenu stickerDetailMenu;
    public EditStickerPanel editStickerPanel;

    public void Awake()
    {
        LoadTransactionListFromFile();
    }

    //public void OpenSavedFavoritesPanel()
    //{
    //    this.gameObject.SetActive(true);
    //    LoadFavoriteStickerListFromFile();
    //}

    private void LoadTransactionListFromFile()
    {
        StickerData stickerData = new StickerData();
        LoadFavoriteStickerList(stickerData.GetAllStickers());
    }

    private void LoadFavoriteStickerList(List<Sticker> stickerList)
    {
    //    foreach (Sticker sticker in stickerList)
    //    {
    //        FavoriteStickerRow row = favoriteStickerList.AddRow();
    //        row.InitiateFavoriteStickerRow(sticker);
    //    }
    }

    //public void OpenFavoriteSticker(Sticker sticker)
    //{
    //    this.gameObject.SetActive(false);
    //    stickerDetailMenu.OpenMenu(sticker);
    //}

    //public void OpenEditFavoriteSticker(Sticker sticker)
    //{
    //    this.gameObject.SetActive(false);
    //    editStickerPanel.OpenEditStickerPanel(sticker);
    //}
}
