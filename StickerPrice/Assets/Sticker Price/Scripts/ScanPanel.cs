using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System.Globalization;

public class ScanPanel : MonoBehaviour
{

    public RawImage scanDisplay;
    public Text currentDateTime;
    public Button ToggleCameraButton;
    public CheckoutPanel checkoutPanel;

    public ItemList itemList;
    //public Rect screenRect;
    private WebCamTexture camTexture;
    private bool scanReady = false; //Is the camera accepting QR code input?
    private float scanTimer; //Time remaining for scanner to be turned on
    [Tooltip("The amount of time the scanner will read after clicking the scan button.")]
    public const float SCAN_TIMER = .25f;
    public ErrorMessage scanError;

    // Use this for initialization
    void Start()
    {
        camTexture = new WebCamTexture(WebCamTexture.devices[0].name, 480, 640, 2);
        scanDisplay.texture = camTexture;
        scanDisplay.material.mainTexture = camTexture;

        camTexture.Play();
    }

    void Update()
    {
        currentDateTime.text = System.DateTime.Now.ToString("F") + ' ' + System.DateTime.Now.ToString("tt");

        if (scanReady)
        {
            scanTimer -= Time.deltaTime;

            IBarcodeReader barcodeReader = new BarcodeReader();
            //decode the current frame
            if (camTexture.isPlaying)
            {
                Result result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

                if (result != null)
                {
                    QRCodeScanned(result);
                }
            }

            if (scanTimer <= 0f)
            {
                scanReady = false;
                scanError.DisplayError(4);
            }
        }
    }

    public void ScanButtonOnClickListener()
    {
        scanReady = true;
        scanTimer = SCAN_TIMER;
    }

    void QRCodeScanned(Result result)
    {
        scanReady = false;
        bool addItem = true; //do we still need to add the item to the list?
        foreach (ItemRow row in itemList.GetItemRows())
        {
            if (row.GetScanString() == result.Text)
            { //Does item already exist in list
                row.SetQuantity(row.GetQuantity() + 1); //item already exists, add quantity
                addItem = false;
            }
        }

        if (addItem)
        { //Check if we still need to add the item to the list or if we already added quantity to an existing item
            String[] resultString = new string[4];
            resultString = result.Text.Split('|');

            float itemPrice = float.Parse(resultString[1], NumberStyles.Currency);

            ItemRow newItem = itemList.AddItem();
            newItem.SetScanString(result.Text);
            newItem.SetItemDescription(resultString[2]);
            newItem.SetItemPrice(itemPrice);
            newItem.SetItemOriginalPrice(itemPrice);
        }

    }

    public void AddItemButtonOnClickListner()
    {
        itemList.AddItem();
    }

    public void ToggleCameraButtonOnClickListener()
    {
        if (camTexture.isPlaying)
        {
            camTexture.Stop();
            ToggleCameraButton.image.color = new Color(1, 1, 1, .8f);
        }
        else
        {
            camTexture.Play();
            ToggleCameraButton.image.color = new Color(1, 1, 1, 0);
        }
    }

    public void CheckoutButtonOnClickListener() {
        this.gameObject.SetActive(false);
        checkoutPanel.gameObject.SetActive(true);

        checkoutPanel.itemList.RemoveAllItems();
        foreach (ItemRow row in itemList.GetItemRows()) {
            checkoutPanel.itemList.AddItem(row);
        }

        checkoutPanel.itemList.SetItemTotal(this.itemList.GetItemTotal());
        checkoutPanel.itemList.SetPriceSubtotal(this.itemList.GetPriceSubtotal());
        checkoutPanel.itemList.SetTaxTotal(this.itemList.GetTaxTotal());
        
    }
}