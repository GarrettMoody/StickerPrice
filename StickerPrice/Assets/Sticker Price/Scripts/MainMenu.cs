using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private Button createButton;
	private Button printButton;
	private Button scanButton;
	private Button trackButton;
    public GameObject createMenu;
    public GameObject scanPanel;


	// Use this for initialization
	void Start () {
		createButton = (Button)this.GetComponent ("CreateButton");
		printButton = (Button)this.GetComponent ("PrintButton");
		scanButton = (Button)this.GetComponent ("ScanButton");
		trackButton = (Button)this.GetComponent ("TrackButton");
	}
	
	public void createButtonOnClickListener() {
        this.gameObject.SetActive(false);
        createMenu.gameObject.SetActive(true);
	}

	public void printButtonOnClickListener() {

	}

	public void scanButtonOnClickListener() {
        this.gameObject.SetActive(false);
        scanPanel.gameObject.SetActive(true);
	}

	public void trackButtonOnClickListener() {

	}

}
