using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public TemplateViewContent templatePrefab;

    // Use this for initialization
    void Awake()
    {
        List<Template> allTemplates = new TemplateData().getAllTemplates();
        allTemplates.ForEach(delegate (Template templateData)
        {
            TemplateViewContent template = (TemplateViewContent)Instantiate(templatePrefab);
            template.InitializeVariables(templateData);
            template.transform.SetParent(scrollContent.transform, false);
        });
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(TemplateViewContent template) {
        this.gameObject.SetActive(false);
        stickerDetailMenu.OpenMenu(template.templateData);
    }
}
