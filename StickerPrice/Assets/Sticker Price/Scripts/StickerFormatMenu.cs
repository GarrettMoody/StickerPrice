using UnityEngine;
using UnityEngine.UI;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public Template templatePrefab;

    public class TemplateStructure
    {
        public string description;
        public string size;
        public string numPerSheet;

        public TemplateStructure(string description, string size, string numPerSheet)
        {
            this.description = description;
            this.size = size;
            this.numPerSheet = numPerSheet;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Template template;
        TemplateStructure[] allTemplates = new TemplateStructure[] {new TemplateStructure("Template - 22805", "1.5 x 1.5", "25 per sheet"),
                                                                  new TemplateStructure("Template - 6450", "1 x 1", "63 per sheet"),
                                                                  new TemplateStructure("Template - 1234", "1 x 1.75", "30 per sheet")};
        for (int i = 0; i < allTemplates.Length; i ++)
        {
            template = (Template)Instantiate(templatePrefab);
            template.initializeVariables(allTemplates[i].description, allTemplates[i].size, allTemplates[i].numPerSheet);
            template.transform.SetParent(scrollContent.transform, false);
        }
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplate22805Clicked() {
        stickerDetailMenu.OpenMenu(22805);
    }

    public void OnTemplate6450Clicked() {
        stickerDetailMenu.OpenMenu(6450);
    }

    public void OnTemplate1234Clicked() {
        stickerDetailMenu.OpenMenu(1234);
    }
}
