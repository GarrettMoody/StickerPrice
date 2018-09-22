using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class StickerDetailMenu : MonoBehaviour {

    //Public Variables
    public ScrollRect scrollRect;
    public GameObject detailsPanel;

    //Private Variables
    public Text templateNumberText;
    public InputField description;
    public InputField numberOfStickers;
    public InputField productOwner;
    public InputField price;
    public ToggleGroup colorCode;
    public GameObject qrPreviewContent;
    private QROption[] qrOptions;

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
}
