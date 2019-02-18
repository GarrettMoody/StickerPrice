using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavoriteStickerRow : ContentRow, IPointerClickHandler
{
    [Header("Favorite Sticker Row Properties")]
    public TextMeshProUGUI stickerName;
    public TextMeshProUGUI templateNumber;
    public TextMeshProUGUI stickerSize;
    public TextMeshProUGUI dateTime;
    public RawImage qrCode;
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
        qrCode.texture = StickerQRCode.CreateQRCode(this.sticker);
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
        //Get the distance between when the mouse was press and when it was released
        Vector2 mouseDistance = eventData.pressPosition - eventData.position;
        Debug.Log(mouseDistance);

        //If the distance was less than 25f (not very far) then it was a click, otherwise it was a drag and we ignore
        if (Mathf.Abs(mouseDistance.x) < 25f && Mathf.Abs(mouseDistance.y) < 25f)
        {
            base.OnPointerClick(eventData);
            favoriteStickerList.savedFavoritesPanel.OpenFavoriteSticker(sticker);
        }
    }


}
