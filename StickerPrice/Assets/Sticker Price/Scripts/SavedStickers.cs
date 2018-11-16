using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class SavedStickers : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public Sticker stickerPrefab;
    public Template templatePrefab;
    public Text fileCount;
    private FileUtility fileUtility = new FileUtility();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Use this for initialization
    void Awake()
    {

        string path = "Assets/Sticker Price/Data Files/SavedStickers.csv";
        List<string> stickers = new List<string>();
        stickers = fileUtility.readFromFile(path);
        stickers.ForEach(delegate (string line)
        {
            string[] values = line.Split(',');
            Sticker sticker = (Sticker)Instantiate(stickerPrefab);
            Template template = getTemplate(values[0]);
            sticker.initializeVariables(template, values[1], values[2], values[3], values[4], values[5], values[6]);
            sticker.transform.SetParent(scrollContent.transform, false);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnStickerClicked(sticker); });
            sticker.transform.GetComponent<EventTrigger>().triggers.Add(entry);
        });
        scrollView.verticalNormalizedPosition = 1;
        fileCount.text = "Number Of Files: " + stickers.Count;
    }

    public Template getTemplate(string templateId)
    {
       
        string path = "Assets/Sticker Price/Data Files/Templates.csv";
        Template template = (Template)Instantiate(templatePrefab);
        //Read template data from the file
        StreamReader reader = new StreamReader(path);
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            string[] values = line.Split(',');
            if (values[0] == templateId)
            {
                template.InitializeVariables(values[0], values[1] + "\" x " + values[2] + "\"", values[3]);
                break;
            }
        }
        reader.Close();
        return template;
    }

    public void OnStickerClicked(Sticker sticker)
    {
        this.gameObject.SetActive(false);
        stickerDetailMenu.OpenMenu(sticker);
    }
}
