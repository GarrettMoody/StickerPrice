using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FavoriteStickerRow : ContentRow, IPointerClickHandler
{
    [Header("Favorite Sticker Row Properties")]
    public TextMeshProUGUI stickerName;
    public TextMeshProUGUI templateNumber;
    public TextMeshProUGUI stickerSize;
    public TextMeshProUGUI dateTime;
    public Sticker sticker;

    private FavoriteStickerList favoriteStickerList;

    public new void Awake()
    {
        base.Awake();
        favoriteStickerList = GetComponentInParent<FavoriteStickerList>();
    }

    public void InitiateFavoriteStickerRow(Sticker sticker)
    {
        this.sticker = sticker;
        stickerName.text = sticker.stickerName;
        templateNumber.text = sticker.template.templateId;
        stickerSize.text = sticker.template.size;
        dateTime.text = sticker.dateSaved;
    }

    public override void OnDefaultButtonClick()
    {
        DeleteButtonOnClickListener();
    }

    public void DeleteButtonOnClickListener()
    {
        parentList.RemoveRow(this);
        StickerData stickerData = new StickerData();
        stickerData.DeleteSticker(sticker);
        
    }

    public new void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        favoriteStickerList.savedFavoritesPanel.stickerDetailMenu.OpenMenu(sticker);
    }


}
