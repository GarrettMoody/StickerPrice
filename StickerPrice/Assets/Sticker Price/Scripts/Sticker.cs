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
    public Template template;

    public Sticker()
    {
        stickerName = "";
        itemDescription = "";
        price = "";
        dateSaved = "";
        owner = "";
        quantity = "";
        template = new Template();
    }

    public Sticker(string stickerDescription, string itemDescription, string price, string dateSaved, string owner, string quantity, Template template)
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
