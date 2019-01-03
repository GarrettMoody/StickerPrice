using UnityEngine;
using UnityEngine.UI;

public class Template : MonoBehaviour
{
    public Text title;
    public Text size;
    public Text numPerSheet;
    public Image qrCode;
    public TemplateData templateData;

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

    public void InitializeVariables(TemplateData templateData)
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