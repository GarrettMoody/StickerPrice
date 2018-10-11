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

    public void OnDescriptionToggle() {
        foreach(QROption option in qrOptions) {
            option.description.gameObject.SetActive(descriptionVisible.isOn);
        }
    }

    public void OnOwnerToggle() {
        foreach (QROption option in qrOptions)
        {
            option.productOwner.gameObject.SetActive(ownerVisible.isOn);
        }
    }

    public void OnPriceToggle() {
        foreach (QROption option in qrOptions)
        {
            option.price.gameObject.SetActive(priceVisible.isOn);
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
