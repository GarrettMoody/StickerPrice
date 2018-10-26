using System.Collections.Generic;
using System.IO;
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

        public TemplateStructure(string  templateId, string length, string width, string numPerSheet)
        {
            this.description = "Template - " + templateId;
            this.size = length + "\" x " + width + "\"";
            this.numPerSheet = numPerSheet;
        }
    }

    // Use this for initialization
    void Awake()
    {

        string path = "Assets/Sticker Price/Data Files/Templates.csv";
        List<TemplateStructure> allTemplates = new List<TemplateStructure>();

        //Read template data from the file
        StreamReader reader = new StreamReader(path);        
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            string[] values = line.Split(',');
            allTemplates.Add(new TemplateStructure(values[0], values[1], values[2], values[3]));
        }
        reader.Close();

        foreach (TemplateStructure templateData in allTemplates)
        {
            Template template = (Template)Instantiate(templatePrefab);
            template.initializeVariables(templateData.description, templateData.size, templateData.numPerSheet);
            template.transform.SetParent(scrollContent.transform, false);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnTemplateClicked(template); });
            template.transform.GetComponent<EventTrigger>().triggers.Add(entry);
        }
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(Template template) {
        stickerDetailMenu.OpenMenu(template);
    }
}
