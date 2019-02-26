using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditStickerPanel : MonoBehaviour
{
    public ContentScroll contentScroll;
    public InputField descriptionInputField;
    public InputField ownerInputField;
    public InputField imageInputField;
    public InputField priceInputField;
    public QROption qrOptionPrefab;
    public Button saveButton;

    private List<Sticker> stickersFromFile;
    private bool isCurrentStickerDirty;

    void OnEnable()
    {
        //Subscribe to the OnSelectionChange event
        contentScroll.OnSelectionChange += UpdateDetailsPanel;
    }

    void OnDisable()
    {
        //Unsubscribe to the OnSelectionChange
        contentScroll.OnSelectionChange -= UpdateDetailsPanel;
    }

    public void OpenEditStickerPanel(Sticker loadSticker)
    {
        this.gameObject.SetActive(true);
        StickerData stickerData = new StickerData();

        contentScroll.RemoveContentComponents();

        stickersFromFile = stickerData.GetAllStickers();
        //Create a QROption for each sticker in the JSON file
        int i = 0;
        foreach (Sticker sticker in stickerData.GetAllStickers())
        {
            QROption newOption = Instantiate(qrOptionPrefab, contentScroll.content.transform);
            newOption.SetSticker(sticker);
            newOption.transform.localPosition = new Vector3(575 + (i * 150), newOption.transform.localPosition.y);
            i++;
        }

        //Set size of content box
        RectTransform rectTransform = contentScroll.content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1500 + (i * 100), 0);

        //Initialize scroll variables
        contentScroll.InitializeVariables();

        //Scroll to selected sticker
        List<Sticker> stickers = stickerData.GetAllStickers();
        int index = stickers.FindIndex(x => x.stickerName == loadSticker.stickerName);

        contentScroll.ScrollToContent(index);
        UpdateDetailsPanel();
        OnDetailsChanged();
    }

    public void UpdateDetailsPanel()
    { //This method updates the Details panel with the data from the QR Code
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if(option != null)
        {
            descriptionInputField.DeactivateInputField();
            ownerInputField.DeactivateInputField();
            descriptionInputField.text = option.description.text;
            ownerInputField.text = option.productOwner.text;
            priceInputField.text = option.price.text;
        }
    }

    public void OnSaveButtonClick()
    {
        //Get stickers from the contentScroll
        Sticker[] stickers = new Sticker[contentScroll.GetContentComponents().Length];
        for (int i = 0; i < contentScroll.GetContentComponents().Length; i++)
        {
            Sticker sticker = contentScroll.GetContentComponents()[i].GetComponent<QROption>().GetSticker();
            if (sticker != null)
            {
                stickers[i] = sticker;
            }
        }

        //Add stickers to JSON file
        StickerData stickerData = new StickerData();
        foreach (Sticker sticker in stickers)
        {
            stickerData.AddSticker(sticker);
        }
    }

    public void OnDescriptionChange()
    { 
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if(option != null)
        {
            option.SetDescription(descriptionInputField.text);
        }
        OnDetailsChanged();
    }

    public void OnProductOwnerChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            option.SetProductionOwner(ownerInputField.text);
        }
        OnDetailsChanged();
    }

    public void OnPriceChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            option.SetPrice(priceInputField.text);
        }
        OnDetailsChanged();
    }

    private void OnDetailsChanged()
    {
        isCurrentStickerDirty = false;
        Sticker stickerFromFile = stickersFromFile[contentScroll.GetSelectedIndex()];
        if(stickerFromFile.itemDescription != descriptionInputField.text)
        {
            isCurrentStickerDirty = true;
        } 
        else if(stickerFromFile.owner != ownerInputField.text)
        {
            isCurrentStickerDirty = true;
        }
        else if(stickerFromFile.price != priceInputField.text)
        {
            isCurrentStickerDirty = true;
        }
        else
        {
            isCurrentStickerDirty = false;
        }

        saveButton.interactable = isCurrentStickerDirty;
    }
}
