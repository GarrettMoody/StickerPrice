using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;

public class StickerDetailMenu : MonoBehaviour
{

    //Public Variables
    public GameObject detailsPanel;
    public SaveToFavoritesPopup saveToFavoritesPopup;
    public Text numberPerSheet;
    public InputField description;
    public InputField quantity;
    public InputField productOwner;
    public InputField price;
    public ToggleGroup colorCode;
    public string stickerDescription;
    public GameObject qrPreviewContent;

    //Private Variables
    private QROption[] qrOptions;
    private Template template;
    private int pageCount = 1;
    private int currentPage = 1;
    private int numberInSheet = 0;
    private int qtyAdded = 0;
    private int qtyLeft = 0;

    //Constants
    readonly Color32 THEME_GREEN = new Color32(0x5C, 0xAB, 0x40, 0xFF);
    readonly Color32 RED = new Color32(0xE2, 0x23, 0x1A, 0xFF);
    readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    readonly Color32 DARK_GREY = new Color32(0x52, 0x53, 0x49, 0xFF);


    // Use this for initialization
    void Awake()
    {

        qrOptions = qrPreviewContent.GetComponentsInChildren<QROption>();

        Texture2D encoded = new Texture2D(256, 256);
        Color32[] color32 = Encode("I Love Sticker Price", encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();

        foreach (QROption option in qrOptions)
        {
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

    public void OnDescriptionChanged()
    {
        UpdateQRCode();
        foreach (QROption option in qrOptions)
        {
            option.SetDescription(description.text);

        }
        saveToFavoritesPopup.saveName.text = description.text;
    }

    public void OnNumberOfStickersChanged()
    {
        UpdateQRCode();
    }

    public void OnProductOwnerChanged()
    {
        UpdateQRCode();
        foreach (QROption option in qrOptions)
        {
            option.SetProductionOwner(productOwner.text);
        }
    }

    public void OnPriceChanged()
    {
        //price.text = "$" + price.text;
        UpdateQRCode();
        foreach (QROption option in qrOptions)
        {
            option.SetPrice(price.text);
        }
    }

    public void OnColorCodeChanged()
    {
        UpdateQRCode();
    }

    private void UpdateQRCode()
    {
        Texture2D encoded = new Texture2D(256, 256);
        Color32[] newCode = Encode("I Love Sticker Price|" + price.text + "|" + description.text + "|" + productOwner.text, 256, 256);
        encoded.SetPixels32(newCode);
        encoded.Apply();
        foreach (QROption option in qrOptions)
        {
            option.GetComponentInChildren<RawImage>().texture = encoded;
        }
    }

    public void OpenMenu(Template template)
    {
        this.template = template;
        this.gameObject.SetActive(true);
        UpdateNumberPerSheetText();
        UpdateInputFields(new Sticker());
    }

    public void OpenMenu(StickerViewContent sticker)
    {
        template = sticker.template;
        this.gameObject.SetActive(true);
        UpdateNumberPerSheetText();
        UpdateInputFields(sticker.stickerData);
    }

    public void UpdateInputFields(Sticker stickerData)
    {
        description.text = stickerData.itemDescription;
        productOwner.text = stickerData.owner;
        price.text = stickerData.price;
        quantity.text = int.Parse(!string.IsNullOrEmpty(stickerData.quantity) ? stickerData.quantity : "0").ToString();
        saveToFavoritesPopup.saveName.text = stickerData.stickerName;
    }

    public void OnQuantityAddButtonClick()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
        if (qty < 999999)
        {
            quantity.text = (qty + 1).ToString();
        }
        else
        {
            quantity.text = "999999";
        }
    }

    public void OnQuantityMinusButtonClick()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantity.text.ToString()) ? quantity.text.ToString() : "0");
        if (qty > 0)
        {
            quantity.text = (qty - 1).ToString();
        }
        else
        {
            quantity.text = "0";
        }
    }

    public void OnQuantityChanged()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
        int numPerSheet = int.Parse(template.numberPerSheet);
        qtyAdded = 0;
        qtyLeft = 0;
        if (qty > numPerSheet)
        {
            pageCount = (int)Math.Ceiling((double)qty / numPerSheet);
            numberInSheet = numPerSheet;
        }
        else
        {
            pageCount = 1;
            currentPage = 1;
            numberInSheet = qty;
        }
        UpdateNumberPerSheetText();
    }

    public void OnConfirmButtonClick()
    {      
        //StickerData stickerData = new StickerData(new Sticker(saveToFavoritesPopup.saveName.text, description.text, price.text, DateTime.Now.ToString("dd MMMM yyyy h:mm tt"), productOwner.text
        // , quantity.text, templateData.templateId));
        //stickerData.CreateSticker();
        
    }

    public void OnAddToPageButtonClick()
    {
        if (pageCount > currentPage)
        {
            currentPage = currentPage + 1;
            int qty = int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
            int numPerSheet = int.Parse(template.numberPerSheet);
            if (qty > numPerSheet)
            {
                qtyAdded = qtyAdded + numPerSheet;
                qtyLeft = qty - qtyAdded;
                numberInSheet = qtyLeft > numPerSheet ? numPerSheet : qtyLeft;
            }
            else
            {
                numberInSheet = qty;
            }
        }
        UpdateNumberPerSheetText();
    }

    public void UpdateNumberPerSheetText()
    {
        string qtyInSheet = numberInSheet != 0 ? numberInSheet.ToString() : template.numberPerSheet;
        numberPerSheet.text = qtyInSheet + " Blank Stickers - Pages " + currentPage + "/" + pageCount;
    }

}
