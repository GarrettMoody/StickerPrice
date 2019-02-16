using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteStickerList : ContentList
{
    public FavoriteStickerRow favoriteStickerRowPrefab;
    public SavedFavoritesPanel savedFavoritesPanel;

    public FavoriteStickerRow AddRow()
    {
        FavoriteStickerRow row = Instantiate(favoriteStickerRowPrefab, contentPanel.transform);
        row.transform.SetAsLastSibling();
        contentList.Add(row);
        ResetAllRows();

        return row;
    }
}
