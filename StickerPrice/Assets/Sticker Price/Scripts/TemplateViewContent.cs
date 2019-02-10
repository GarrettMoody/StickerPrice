using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateViewContent : MonoBehaviour
{
    public Text title;
    public Text size;
    public Text numPerSheet;
    public Image qrCode;
    public Template templateData;

    private StickerFormatMenu stickerFormatMenu;

    public void Start()
    {
        stickerFormatMenu = this.GetComponentInParent<StickerFormatMenu>();
    }

    public void SetTitle()
    {
        title.text = "Template - " + templateData.templateId;
    }

    public void SetSize()
    {
        this.size.text = templateData.size;
    }

    public void SetNumPerSheet()
    {
        this.numPerSheet.text = templateData.numberPerSheet + " Per Sheet";
    }

    public void InitializeVariables(Template templateData)
    {
        this.templateData = templateData;
        SetTitle();
        SetSize();
        SetNumPerSheet();
    }

    public void OnTemplateClick()
    {
        stickerFormatMenu.OnTemplateClicked(this);
    }
}
