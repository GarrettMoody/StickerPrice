using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Template : MonoBehaviour
{
    public Text title;
    public Text size;
    public Text numPerSheet;
    public Image qrCode;
    public string templateId;
    public string numberPerSheet;

    private StickerFormatMenu stickerFormatMenu;

    public void Start()
    {
        stickerFormatMenu = this.GetComponentInParent<StickerFormatMenu>();
    }

    public void SetTitle()
    {
        title.text = "Template - " + templateId;
    }

    public void SetSize(string size)
    {
        this.size.text = size;
    }

    public void SetNumPerSheet()
    {
        this.numPerSheet.text = numberPerSheet + " Per Sheet";
    }

    public void InitializeVariables(string templateId, string size, string numberPerSheet)
    {
        this.templateId = templateId;
        this.numberPerSheet = numberPerSheet;
        SetTitle();
        SetSize(size);
        SetNumPerSheet();
    }

    public void OnTemplateClick()
    {
        stickerFormatMenu.OnTemplateClicked(this);
    }

}