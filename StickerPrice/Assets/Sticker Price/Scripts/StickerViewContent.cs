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
    public Template template;
    public Sticker stickerData;

    public void SetStickerDescription()
    {
        this.stickerDescription.text = stickerData.stickerName;
    }

    public void SetItemDescription()
    {
        this.itemDescription.text = stickerData.itemDescription;
    }

    public void SetNumAndSize()
    {
        numAndSize.text = template.numberPerSheet + " - " + template.size;
    }

    public void SetPrice()
    {
        this.price.text =  stickerData.price;
    }

    public void SetDateSaved()
    {
        this.dateSaved.text = stickerData.dateSaved;
    }

    public void SetColorCode(Color32 colorCode)
    {
        this.colorCode = colorCode;
    }

    public void SetTemplateData(Template template)
    {
        this.template = template;
    }

    public void SetStickerData(Sticker stickerData)
    {
        this.stickerData = stickerData;
    }

    public void InitializeVariables(Template template,Sticker stickerData)
    {
        SetStickerData(stickerData);
        SetTemplateData(template);
        SetStickerDescription();
        SetItemDescription();
        SetPrice();
        SetDateSaved();
    }

}
