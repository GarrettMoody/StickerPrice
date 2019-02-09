using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerViewContent : MonoBehaviour {
    public Text stickerDescription;
    public Text itemDescription;
    public Text numAndSize;
    public Text price;
    public Text dateSaved;
    public Color32 colorCode;
    public Template templateData;
    public Sticker stickerData;

    public void Start()
    {

    }

    public void setStickerDescription()
    {
        this.stickerDescription.text = stickerData.stickerName;
    }

    public void setItemDescription()
    {
        this.itemDescription.text = stickerData.itemDescription;
    }

    public void setNumAndSize()
    {
        numAndSize.text = templateData.numberPerSheet + " - " + templateData.size;
    }

    public void setPrice()
    {
        this.price.text =  stickerData.price;
    }

    public void setDateSaved()
    {
        this.dateSaved.text = stickerData.dateSaved;
    }

    public void setColorCode(Color32 colorCode)
    {
        this.colorCode = colorCode;
    }

    public void setTemplate (Template templateData)
    {
        this.templateData = templateData;
    }

    public void setStickerData (Sticker stickerData)
    {
        this.stickerData = stickerData;
    }

    public void initializeVariables(Template templateData,Sticker stickerData)
    {
        setStickerData(stickerData);
        setTemplate(templateData);
        setStickerDescription();
        setItemDescription();
        setPrice();
        setDateSaved();
    }

}
