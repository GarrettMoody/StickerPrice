using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker
{
    public string stickerName;
    public string itemDescription;
    public string price;
    public string dateSaved;
    public string owner;
    public string quantity;
    public string templateId;

    public Sticker()
    {
        stickerName = "";
        itemDescription = "";
        price = "";
        dateSaved = "";
        owner = "";
        quantity = "";
        templateId = "";
    }

    public Sticker(string stickerDescription, string itemDescription, string price, string dateSaved, string owner, string quantity, string templateId)
    {
        this.stickerName = stickerDescription;
        this.itemDescription = itemDescription;
        this.price = price;
        this.dateSaved = dateSaved;
        this.owner = owner;
        this.quantity = quantity;
        this.templateId = templateId;
    }
}
