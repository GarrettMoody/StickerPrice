﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class ScanPanel : MonoBehaviour
{

	public RawImage scanDisplay;
	public Text currentDateTime;
	public Button ToggleCameraButton;

	private String previousResult;
	private ItemList itemList;
	private Rect screenRect;
	private WebCamTexture camTexture;

	// Use this for initialization
	void Start ()
	{
		camTexture = new WebCamTexture (WebCamTexture.devices [0].name, 480, 640, 2);
		itemList = this.transform.Find ("ContentPanel/ItemList").GetComponent<ItemList> ();
		//camTexture.requestedFPS = 60;
		previousResult = "";
		scanDisplay.texture = camTexture;
		scanDisplay.material.mainTexture = camTexture;
		//camTexture.Play ();
	}

	void Update ()
	{
		currentDateTime.text = System.DateTime.Now.ToString ("F") + ' ' + System.DateTime.Now.ToString ("tt");
		IBarcodeReader barcodeReader = new BarcodeReader ();
		//decode the current frame
		if (camTexture.isPlaying) {
			Result result = barcodeReader.Decode (camTexture.GetPixels32 (), camTexture.width, camTexture.height);

			if (result != null) {
				QRCodeScanned (result);
			}
		}
	}

	public void ScanButtonOnClickListener ()
	{
		if (camTexture != null) {
			camTexture.Play ();
		}
	}

	void QRCodeScanned (Result result)
	{
		//Debug.Log (result.Text);
		//camTexture.Stop ();

		//ItemRow newItem = Instantiate (itemPrefab);
		//newItem.transform.parent = this.transform.Find ("ContentPanel/ItemList/Viewport/ContentPanel").gameObject.transform;
		if (previousResult != result.Text) {
			previousResult = result.Text;

			String[] resultString = new string[2];
			resultString = result.Text.Split ('|');

			float itemPrice = float.Parse (resultString [0]);

			ItemRow newItem = itemList.addItem ();
			newItem.setItemDescription (resultString [1]);
			newItem.setItemPrice (itemPrice);
		}
	}

	public void AddItemButtonOnClickListner ()
	{
		itemList.addItem ();
	}

	public void ToggleCameraButtonOnClickListener ()
	{
		if (camTexture.isPlaying) {
			camTexture.Stop ();
			//ToggleCameraButton.GetComponent<Image>().color = new Color (255, 255, 255, 180);
			ToggleCameraButton.image.color = new Color (1, 1, 1, .8f);
		} else {
			camTexture.Play ();
			//ToggleCameraButton.GetComponent<Image>().color = new Color (255, 255, 255, 0);
			ToggleCameraButton.image.color = new Color (1, 1, 1, 0);
		}
	}
}