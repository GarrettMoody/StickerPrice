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
    public int quantity;
    public Template template;

    public Sticker()
    {
        stickerName = "";
        itemDescription = "";
        price = "";
        dateSaved = "";
        owner = "";
        quantity = 0;
        template = new Template();
    }

    //Copy Constructor
    public Sticker(Sticker copySticker)
    {
        this.stickerName = copySticker.stickerName;
        this.itemDescription = copySticker.itemDescription;
        this.price = copySticker.price;
        this.dateSaved = copySticker.dateSaved;
        this.owner = copySticker.owner;
        this.quantity = copySticker.quantity;
        this.template = copySticker.template;
    }

    public Sticker(string stickerDescription, string itemDescription, string price, string dateSaved, string owner, int quantity, Template template)
    {
        this.stickerName = stickerDescription;
        this.itemDescription = itemDescription;
        this.price = price;
        this.dateSaved = dateSaved;
        this.owner = owner;
        this.quantity = quantity;
        this.template = template;
    }
}
