using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button createButton;
	public Button printButton;
	public Button scanButton;
    public Button trackButton;
    public Button moreButton;
    public GameObject stickerFormatMenu;
    public GameObject scanPanel;
    public GameObject moreMenu;


	// Use this for initialization
	void Start () {

	}
	
	public void createButtonOnClickListener() {
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
