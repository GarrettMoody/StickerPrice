using System;
using UnityEngine;

public class StickerJson {

    public string stickerDescription;
    public string itemDescription;
    public string price;
    public string dateSaved;
    public string owner;
    public string quantity;
    public string templateId;

    public StickerJson(string stickerDescription, string itemDescription, string price, string dateSaved, string owner, string quantity, string templateId)
    {
        this.stickerDescription = stickerDescription;
        this.itemDescription = itemDescription;
        this.price = price;
        this.dateSaved = dateSaved;
        this.owner = owner;
        this.quantity = quantity;
        this.templateId = templateId;
    }
}
