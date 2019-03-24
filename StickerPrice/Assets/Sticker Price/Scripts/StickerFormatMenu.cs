using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public QROption templatePrefab;

    // Use this for initialization
    void Awake()
    {
        List<Template> allTemplates = new TemplateData().GetAllTemplates();
        allTemplates.ForEach(delegate (Template template)
        {
            QROption templateViewContent = (QROption)Instantiate(templatePrefab);
            //templateViewContent.InitializeVariables(template);
            templateViewContent.transform.SetParent(scrollContent.transform, false);
        });
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(Template template) {
        this.gameObject.SetActive(false);
        stickerDetailMenu.OpenMenu(template);
    }
}
