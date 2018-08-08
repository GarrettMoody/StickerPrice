using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private Button createButton;
	private Button printButton;
	private Button scanButton;
	private Button trackButton;
    private Button moreButton;
    public GameObject stickerFormatMenu;
    public GameObject scanPanel;
    public GameObject moreMenu;


	// Use this for initialization
	void Start () {
        createButton = (Button)this.transform.Find("MenuButtons/CreateButton").gameObject.GetComponent<Button>();
        createButton.onClick.AddListener(createButtonOnClickListener);
        printButton = (Button)this.transform.Find ("MenuButtons/PrintButton").gameObject.GetComponent<Button>();
        printButton.onClick.AddListener(printButtonOnClickListener);
        scanButton = (Button)this.transform.Find ("MenuButtons/ScanButton").gameObject.GetComponent<Button>();
        scanButton.onClick.AddListener(scanButtonOnClickListener);
        trackButton = (Button)this.transform.Find ("MenuButtons/TrackButton").gameObject.GetComponent<Button>();
        trackButton.onClick.AddListener(trackButtonOnClickListener);
        moreButton = (Button)this.transform.Find("MoreButton").gameObject.GetComponent<Button>();
        moreButton.onClick.AddListener(moreButtonOnClickListener);
	}
	
	void createButtonOnClickListener() {
        this.gameObject.SetActive(false);
        stickerFormatMenu.gameObject.SetActive(true);
	}

	public void printButtonOnClickListener() {

	}

	public void scanButtonOnClickListener() {
        this.gameObject.SetActive(false);
        scanPanel.gameObject.SetActive(true);
	}

	public void trackButtonOnClickListener() {

	}

    public void moreButtonOnClickListener() {
        this.gameObject.SetActive(false);
        moreMenu.gameObject.SetActive(true);
    }
}
