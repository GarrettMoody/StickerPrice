using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedFavoritesPanel : MonoBehaviour
{
    public ToggleGroup favoriteType;
    public Toggle stickerToggle;
    public Toggle pagesToggle;
    public Button clearButton;
    public Button filterButton;
    public Button sortButton;
    public InputField searchInputField;
    public FavoriteStickerList favoriteStickerList;
    public FavoriteStickerRow favoriteStickerRowPrefab;
    public StickerDetailMenu stickerDetailMenu;
    public EditStickerPanel editStickerPanel;

    private void Start()
    {
        LoadFavoriteStickerListFromFile();
    }

    public void OpenSaveFavoritesPanel()
    {
        this.gameObject.SetActive(true);
        LoadFavoriteStickerListFromFile();
    }

    private void LoadFavoriteStickerListFromFile()
    {
        StickerData stickerData = new StickerData();
        LoadFavoriteStickerList(stickerData.GetAllStickers());
    }

    private void LoadFavoriteStickerList(List<Sticker> stickerList)
    {
        favoriteStickerList.RemoveAllRows();
        foreach (Sticker sticker in stickerList)
        {
            FavoriteStickerRow row = favoriteStickerList.AddRow();
            row.InitiateFavoriteStickerRow(sticker);
        }
    }

    public void OnSearchInputFieldChangeListener()
    {
        if (searchInputField.text != "")
        {
            StickerData stickerData = new StickerData();
            List<Sticker> stickers = stickerData.GetAllStickers();

            //Filter list based on sticker name
            List<Sticker> filteredList = stickers.FindAll(x => x.stickerName.ToLower().Contains(searchInputField.text.ToLower()));
            LoadFavoriteStickerList(filteredList);
        }
        else
        {
            LoadFavoriteStickerListFromFile();
        }
    }

    public void OnClearButtonClick()
    {
        searchInputField.text = "";
    }

    public void OpenFavoriteSticker(Sticker sticker)
    {
        this.gameObject.SetActive(false);
        stickerDetailMenu.OpenMenu(sticker);
    }

    public void OpenEditFavoriteSticker(Sticker sticker)
    {
        this.gameObject.SetActive(false);
        editStickerPanel.OpenEditStickerPanel(sticker);
    }
}
