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
    public Template template;

    private StickerFormatMenu stickerFormatMenu;

    public void Start()
    {
        stickerFormatMenu = this.GetComponentInParent<StickerFormatMenu>();
    }

    public void SetTitle()
    {
        title.text = "Template - " + template.templateId;
    }

    public void SetSize()
    {
        this.size.text = template.size;
    }

    public void SetNumPerSheet()
    {
        this.numPerSheet.text = template.numberPerSheet + " Per Sheet";
    }

    public void InitializeVariables(Template template)
    {
        this.template = template;
        SetTitle();
        SetSize();
        SetNumPerSheet();
    }

    public void OnTemplateClick()
    {
        stickerFormatMenu.OnTemplateClicked(template);
    }
}
