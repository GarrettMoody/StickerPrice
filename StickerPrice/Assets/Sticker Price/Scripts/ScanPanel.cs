using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class ScanPanel : MonoBehaviour {

	public RawImage scanDisplay;
	public Text currentDateTime;
	public Button ToggleCameraButton;
	public GameObject itemPrefab;

	private Rect screenRect;
	private WebCamTexture camTexture;

	// Use this for initialization
	void Start() {
		camTexture = new WebCamTexture (WebCamTexture.devices[0].name, 480, 640, 30);
		//camTexture.requestedFPS = 60;
		scanDisplay.texture = camTexture;
		scanDisplay.material.mainTexture = camTexture;
		//camTexture.Play ();
	}

	void Update() {


		currentDateTime.text = System.DateTime.Now.ToString ("F") + ' ' + System.DateTime.Now.ToString("tt");
		IBarcodeReader barcodeReader = new BarcodeReader ();
		//decode the current frame
		if (camTexture.isPlaying) {
			Result result = barcodeReader.Decode (camTexture.GetPixels32 (), camTexture.width, camTexture.height);

			if (result != null) {
				QRCodeScanned (result);
			}
		}
	}

	public void ScanButtonOnClickListener() {
		if (camTexture != null) {
			camTexture.Play();
		}

		Debug.Log ("");
	}

	void QRCodeScanned(Result result) {
		Debug.Log (result.Text);
		camTexture.Stop ();

		GameObject newItem = Instantiate (itemPrefab);
		newItem.transform.parent = this.transform.Find ("ContentPanel/ItemList/Viewport/ContentPanel").gameObject.transform;

		Text itemText =	newItem.GetComponentInChildren<Text>();
		itemText.text = result.Text;
	}

	public void AddItemButtonOnClickListner() {
		GameObject newItem = Instantiate (itemPrefab);
		newItem.transform.parent = this.transform.Find ("ContentPanel/ItemList/Viewport/ContentPanel").gameObject.transform;
	}

	public void ToggleCameraButtonOnClickListener() {
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