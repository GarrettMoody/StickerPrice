using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour {

    public Button createButton;
    public Button printButton;
    public Button scanButton;
    public Button trackButton;
    public Button moreButton;
    public Canvas canvas;
    public GameObject stickerFormatMenu;
    public GameObject scanPanel;
    public GameObject readyForPrint;
    public GameObject moreMenu;

    public void CreateButtonOnClickListener()
    {
        DeactivateAllPages();
        stickerFormatMenu.gameObject.SetActive(true);
    }

    public void PrintButtonOnClickListener()
    {
       // DeactivateAllPages();
    }

    public void ScanButtonOnClickListener()
    {
        DeactivateAllPages();
        scanPanel.gameObject.SetActive(true);
    }

    public void TrackButtonOnClickListener()
    {
        DeactivateAllPages();
    }

    public void MoreButtonOnClickListener()
    {
        DeactivateAllPages();
        moreMenu.gameObject.SetActive(true);
    }

    private void DeactivateAllPages() {
        foreach(Transform page in canvas.transform) {
            if(page.gameObject != this.gameObject)
            {
                page.gameObject.SetActive(false);
            }
        }
    }
}
