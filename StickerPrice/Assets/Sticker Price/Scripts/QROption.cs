using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QROption : MonoBehaviour
{
    public Text description;
    public Text productOwner;
    public Text price;
    public RawImage qrCode;
    public Text numberOfStickers;
    private Sticker sticker;

    public void SetDescription(string value)
    {
        description.text = value;
        if (sticker != null)
        {
            sticker.itemDescription = value;
            UpdateQRCode();
        }
    }

    public void SetProductionOwner(string value)
    {
        productOwner.text = value;
        if (sticker != null)
        {
            sticker.owner = value;
            UpdateQRCode();
        }
    }

    public void SetPrice(string value) {
        price.text = value;
        if (sticker != null)
        {
            sticker.price = value;
            UpdateQRCode();
        }
    }

    public void SetNumberOfStickers(string value) {
        numberOfStickers.text = value;
        if (sticker != null)
        {
            sticker.quantity = value;
            UpdateQRCode();
        }
    }

    public void UpdateQRCode()
    {
        qrCode.texture = StickerQRCode.CreateQRCode(sticker);
    }

    public void SetSticker(Sticker sticker)
    {
        this.sticker = sticker;
        SetDescription(sticker.itemDescription);
        SetProductionOwner(sticker.owner);
        SetPrice(sticker.price);
        SetNumberOfStickers(sticker.quantity);
    }

    public Sticker GetSticker()
    {
        return sticker;
    }
}
