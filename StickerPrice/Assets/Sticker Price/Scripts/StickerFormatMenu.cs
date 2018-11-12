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

    // Use this for initialization
    void Awake()
    {

        string path = "Assets/Sticker Price/Data Files/Templates.csv";

        //Read template data from the file
        StreamReader reader = new StreamReader(path);        
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            string[] values = line.Split(',');
            Template template = (Template)Instantiate(templatePrefab);
            template.InitializeVariables(values[0], values[1] + "\" x " + values[2] + "\"", values[3]);
            template.transform.SetParent(scrollContent.transform, false);
            //EventTrigger.Entry entry = new EventTrigger.Entry();
            //entry.eventID = EventTriggerType.PointerClick;
            //entry.callback.AddListener((eventData) => { OnTemplateClicked(template); });
            //template.transform.GetComponent<EventTrigger>().triggers.Add(entry);
        }
        reader.Close();
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplateClicked(Template template) {
        stickerDetailMenu.OpenMenu(template);
    }
}
