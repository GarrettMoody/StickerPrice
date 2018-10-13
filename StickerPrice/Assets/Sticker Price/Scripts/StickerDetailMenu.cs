﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using UnityEngine.EventSystems;

public class StickerDetailMenu : MonoBehaviour {

    //Public Variables
    public ScrollRect scrollRect;
    public GameObject detailsPanel;

    //Private Variables
    public Text templateNumberText;
    public InputField description;
    public InputField quantity;
    public InputField productOwner;
    public InputField price;
    public ToggleGroup colorCode;
    public GameObject qrPreviewContent;
    public Toggle descriptionVisible;
    public Toggle ownerVisible;
    public Toggle priceVisible;
    private QROption[] qrOptions;
    public InputField Quantity
    {
        get
        {
            return quantity;
        }

        set
        {
            quantity = value;
        }
    }

    //Constants
    readonly Color32 THEME_GREEN = new Color32(0x5C, 0xAB, 0x40, 0xFF);
    readonly Color32 RED = new Color32(0xE2, 0x23, 0x1A, 0xFF);
    readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    readonly Color32 DARK_GREY = new Color32(0x52, 0x53, 0x49, 0xFF);


    // Use this for initialization
    void Awake () {

        qrOptions = qrPreviewContent.GetComponentsInChildren<QROption>();

        Texture2D encoded = new Texture2D (256, 256);   
        Color32[] color32 = Encode("I Love Sticker Price", encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();

        foreach(QROption option in qrOptions) {
            option.GetComponentInChildren<RawImage>().texture = encoded;
        }
        ChangeToggleColor(descriptionVisible);
        ChangeToggleColor(ownerVisible);
        ChangeToggleColor(priceVisible);
	}

    // for generate qrcode
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public void OnDescriptionChanged() {
        UpdateQRCode();
        foreach(QROption option in qrOptions) {
            option.setDescription(description.text);

        }
    }

    public void OnNumberOfStickersChanged() {
        UpdateQRCode();
    }

    public void OnProductOwnerChanged() {
        UpdateQRCode();
        foreach (QROption option in qrOptions)
        {
            option.setProductionOwner(productOwner.text);
        }
    }

    public void OnPriceChanged() {
        UpdateQRCode();
        foreach (QROption option in qrOptions)
        {
            option.setPrice(price.text);
        }
    }

    public void OnColorCodeChanged() {
        UpdateQRCode();
    }

    private void UpdateQRCode() {
        Texture2D encoded = new Texture2D (256, 256); 
        Color32[] newCode = Encode("I Love Sticker Price|" + price.text + "|" + description.text + "|" + productOwner.text, 256, 256);
        encoded.SetPixels32(newCode);
        encoded.Apply();
        foreach (QROption option in qrOptions)
        {
            option.GetComponentInChildren<RawImage>().texture = encoded;
        }
    }

    public void OpenMenu(int templateNumber) {
        this.gameObject.SetActive(true);
        templateNumberText.text = "Template - " + templateNumber.ToString();

    }

    public void OnDescriptionToggle() {
        foreach(QROption option in qrOptions) {
            option.description.gameObject.SetActive(descriptionVisible.isOn);
        }
        ChangeToggleColor(descriptionVisible);
    }

    public void OnOwnerToggle() {
        foreach (QROption option in qrOptions)
        {
            option.productOwner.gameObject.SetActive(ownerVisible.isOn);
        }
        ChangeToggleColor(ownerVisible);
    }

    public void OnPriceToggle() {
        foreach (QROption option in qrOptions)
        {
            option.price.gameObject.SetActive(priceVisible.isOn);
        }
        ChangeToggleColor(priceVisible);
    }

    void ChangeToggleColor(Toggle toggle) {
        if (toggle.isOn)
        {
            ColorBlock colors = descriptionVisible.colors;
            colors.normalColor = THEME_GREEN;

            toggle.colors = colors;
        }
        else
        {
            ColorBlock colors = toggle.colors;
            colors.normalColor = WHITE;

            toggle.colors = colors;
        }

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnQuantityAddButtonClick() 
    {
        int qty = int.Parse(quantity.text != null && quantity.text != "" ? quantity.text : "0" );
        if (qty < 999999)
        {
            quantity.text = (qty + 1).ToString();
        } else
        {
            quantity.text = "999999";
        }
    }

    public void OnQuantityMinusButtonClick()
    {
        int qty = int.Parse(quantity.text != null && quantity.text != "" ? quantity.text : "0");
        if (qty > 0)
        {
            quantity.text = (qty - 1).ToString();
        }
        else
        {
            quantity.text = "0";
        }
    }
}