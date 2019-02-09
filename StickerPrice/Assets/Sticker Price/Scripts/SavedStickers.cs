﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SavedStickers : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public StickerViewContent stickerPrefab;
    public Text fileCount;
    private FileUtility fileUtility = new FileUtility();

    // Use this for initialization
    void Awake()
    {
        //List<Sticker> allStickers = new StickerData(new Sticker()).getAllStickers();
        //allStickers.ForEach(delegate (Sticker stickerData)
        {
            StickerViewContent sticker = (StickerViewContent)Instantiate(stickerPrefab);
            //sticker.initializeVariables(new TemplateData(stickerData.templateId).getTemplate(), stickerData);
            sticker.transform.SetParent(scrollContent.transform, false);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnStickerClicked(sticker); });
            sticker.transform.GetComponent<EventTrigger>().triggers.Add(entry);
        }
        scrollView.verticalNormalizedPosition = 1;
        //fileCount.text = "Number Of Files: " + allStickers.Count;
    }

    public void OnStickerClicked(StickerViewContent sticker)
    {
        this.gameObject.SetActive(false);
        stickerDetailMenu.OpenMenu(sticker);
    }
}
