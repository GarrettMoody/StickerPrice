using UnityEngine;
using UnityEngine.EventSystems;
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

        public TemplateStructure(int templateId, string size, string numPerSheet)
        {
            this.description = "Template - " + templateId;
            this.size = size;
            this.numPerSheet = numPerSheet;
        }
    }

    // Use this for initialization
    void Awake()
    {
        TemplateStructure[] allTemplates = new TemplateStructure[] {new TemplateStructure(22805, "1.5 x 1.5", "25 per sheet"),
                                                                  new TemplateStructure(6450, "1 x 1", "63 per sheet"),
                                                                  new TemplateStructure(1234, "1 x 1.75", "30 per sheet")};
        for (int i = 0; i < allTemplates.Length; i ++)
        {
            Template template = (Template)Instantiate(templatePrefab);
            template.initializeVariables(allTemplates[i].description, allTemplates[i].size, allTemplates[i].numPerSheet);
            template.transform.SetParent(scrollContent.transform, false);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnTemplateClicked(template.title.text); });
            template.transform.GetComponent<EventTrigger>().triggers.Add(entry);
        }
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(string description) {
        stickerDetailMenu.OpenMenu(description);
    }
}
