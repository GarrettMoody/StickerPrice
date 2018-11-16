using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sticker : MonoBehaviour {
    public Text stickerDescription;
    public Text itemDescription;
    public Text numAndSize;
    public Text price;
    public Text dateSaved;
    public string owner;
    public string quantity;
    public Color32 colorCode;
    public string color;
    public Template template;

    public void Start()
    {

    }

    public void setStickerDescription(string stickerDescription)
    {
        this.stickerDescription.text = stickerDescription;
    }

    public void setItemDescription(string itemDescription)
    {
        this.itemDescription.text = itemDescription;
    }

    public void setNumAndSize()
    {
        numAndSize.text = template.numberPerSheet + " - " + template.size;
    }

    public void setPrice(string price)
    {
        this.price.text = "$" + price;
    }

    public void setDateSaved(string dateSaved)
    {
        this.dateSaved.text = dateSaved;
    }

    public void setOwner(string owner)
    {
        this.owner = owner;
    }

    public void setColorCode(Color32 colorCode)
    {
        this.colorCode = colorCode;
    }

    public void setTemplate (Template template)
    {
        this.template = template;
    }

    public void setQuantity (string quantity)
    {
        this.quantity = quantity;
    }

    public void initializeVariables(Template template, string stickerDescription, string itemDescription, string owner, string price, 
                                    string quantity, string dateSaved)
    {
        setStickerDescription(stickerDescription);
        setItemDescription(itemDescription);
        setPrice(price);
        setOwner(owner);
        setDateSaved(dateSaved);
        setQuantity(quantity);
        setTemplate(template);
    }

}
