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
    public PopupMessage stickerSavedPopup;
    public EditPagePanel editPagePanel;
    public Button addToPageButton;
    public Button editPageButton;

    //Private Variables
    private QROption[] qrOptions;
    private Template template;
    private StickerPage stickerPage;

    //Constants
    readonly Color32 THEME_GREEN = new Color32(0x5C, 0xAB, 0x40, 0xFF);
    readonly Color32 RED = new Color32(0xE2, 0x23, 0x1A, 0xFF);
    readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    readonly Color32 DARK_GREY = new Color32(0x52, 0x53, 0x49, 0xFF);


    // Use this for initialization
    void Awake()
    {
        UpdateNumberPerSheetText();
        qrOptions = qrPreviewContent.GetComponentsInChildren<QROption>();

        Sticker sticker = new Sticker("", description.text, price.text, "", productOwner.text, 0, template);

        foreach (QROption option in qrOptions)
        {
            option.GetComponentInChildren<RawImage>().texture = StickerQRCode.CreateQRCode(sticker);
        }
    }

    private void OnDisable()
    {
        stickerSavedPopup.ClosePopup(true);
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
        Sticker sticker = new Sticker("", description.text, price.text, "", productOwner.text, 0, template);
        foreach (QROption option in qrOptions)
        {
            option.GetComponentInChildren<RawImage>().texture = StickerQRCode.CreateQRCode(sticker);
        }
    }

    public void OpenMenu(Template template)
    {
        this.template = template;
        stickerPage = new StickerPage(template);
        this.gameObject.SetActive(true);
        UpdateNumberPerSheetText();
        UpdateInputFields(new Sticker());

        if (stickerPage.stickers.Count > 0)
        {
            editPageButton.interactable = true;
        }
        else
        {
            editPageButton.interactable = false;
        }

        UpdateAddToPageButton();
    }

    public void OpenMenu(Sticker sticker)
    {
        template = sticker.template;
        stickerPage = new StickerPage(template);
        this.gameObject.SetActive(true);
        SetStickersOnOptions(sticker);
        UpdateNumberPerSheetText();
        UpdateInputFields(sticker);

        if (stickerPage.stickers.Count > 0)
        {
            editPageButton.interactable = true;
        }
        else
        {
            editPageButton.interactable = false;
        }

        UpdateAddToPageButton();
    }

    public void SetStickersOnOptions(Sticker sticker)
    {
        if(qrOptions != null)
        {
            foreach(QROption option in qrOptions)
            {
                option.SetSticker(sticker);
            }
        }
    }

    public void UpdateInputFields(Sticker sticker)
    {
        description.text = sticker.itemDescription;
        productOwner.text = sticker.owner;
        price.text = sticker.price;
        quantity.text = sticker.quantity.ToString();
        saveToFavoritesPopup.saveName.text = sticker.stickerName;
    }

    public void OnQuantityChange()
    {
        UpdateAddToPageButton();
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

        UpdateAddToPageButton();
    }

    public void OnQuantityMinusButtonClick()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
        if (qty > 0)
        {
            quantity.text = (qty - 1).ToString();
        }
        else
        {
            quantity.text = "0";
        }

        UpdateAddToPageButton();
    }

    private void UpdateAddToPageButton()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
        if (qty > 0)
        {
            addToPageButton.interactable = true;
        }
        else
        {
            addToPageButton.interactable = false;
        }
    }

    public void OnAddToPageButtonClick()
    {
        Sticker sticker = new Sticker("", description.text, price.text, DateTime.Now.ToString(), productOwner.text, GetQuantity(), template);
        stickerPage.AddSticker(sticker);
        UpdateNumberPerSheetText();
        if (stickerPage.stickers.Count > 0)
        {
            editPageButton.interactable = true;
        }
    }

    public void UpdateNumberPerSheetText()
    {
        //Figure out how many stickers are left on the page
        if(stickerPage != null)
        {
            numberPerSheet.text = stickerPage.GetStickersLeftOnPage() + " Blank Stickers Remaining - Pages " + stickerPage.GetNumberOfPages();
        }
    }

    public void SaveToFavoritesOnClickListener()
    {
        Sticker sticker = new Sticker("", description.text, price.text, DateTime.Now.ToString(), productOwner.text, 0, template);
        saveToFavoritesPopup.OpenSaveToFavoritesPopup(sticker);
    }

    private int GetQuantity()
    {
        return int.Parse(!string.IsNullOrEmpty(quantity.text) ? quantity.text : "0");
    }

    public void OpenEditPagePanel()
    {
        this.gameObject.SetActive(false);
        editPagePanel.OpenEditPagePanel(stickerPage);
    }

    public void SetStickerPage(StickerPage stickerPage)
    {
        this.stickerPage = stickerPage;
    }
}
