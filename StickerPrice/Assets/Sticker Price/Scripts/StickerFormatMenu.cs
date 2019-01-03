using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public Template templatePrefab;

    // Use this for initialization
    void Awake()
    {
        List<TemplateData> allTemplates = new TemplateData().getAllTemplates();
        allTemplates.ForEach(delegate (TemplateData templateData)
        {
            Template template = (Template)Instantiate(templatePrefab);
            template.InitializeVariables(templateData);
            template.transform.SetParent(scrollContent.transform, false);
        });
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(Template template) {
        stickerDetailMenu.OpenMenu(template.templateData);
    }
}
