using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	private Button createButton;
	private Button printButton;
	private Button scanButton;
	private Button trackButton;

	// Use this for initialization
	void Start () {
		createButton = (Button)this.GetComponent ("CreateButton");
		printButton = (Button)this.GetComponent ("PrintButton");
		scanButton = (Button)this.GetComponent ("ScanButton");
		trackButton = (Button)this.GetComponent ("TrackButton");
	}
	
	public void createButtonOnClickListener() {

	}

	public void printButtonOnClickListener() {

	}

	public void scanButtonOnClickListener() {

	}

	public void trackButtonOnClickListener() {

	}

}
