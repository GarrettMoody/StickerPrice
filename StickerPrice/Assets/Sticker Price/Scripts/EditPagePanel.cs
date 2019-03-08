using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditPagePanel : MonoBehaviour
{
    public ContentScroll contentScroll;
    public QROption qrOptionPrefab;
    public InputField descriptionInputField;
    public InputField ownerInputField;
    public InputField priceInputField;
    public InputField quantityInputField;
    public Button saveButton;
    public StickerDetailMenu stickerDetailMenu;

    private List<Sticker> lastSavedStickers = new List<Sticker>(); //The last saved sticker page. Used to see if page is dirty

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

    public void OpenEditPagePanel(StickerPage stickerPage)
    {
        this.gameObject.SetActive(true);

        lastSavedStickers = new List<Sticker>();
        foreach(Sticker copySticker in stickerPage.stickers)
        {
            lastSavedStickers.Add(new Sticker(copySticker));
        }

        contentScroll.RemoveContentComponents();
        //Create a QROption for each sticker
        int i = 0;
        foreach (Sticker sticker in stickerPage.stickers)
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
    }

    public void UpdateDetailsPanel()
    { //This method updates the Details panel with the data from the QR Code
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            descriptionInputField.DeactivateInputField();
            ownerInputField.DeactivateInputField();
            descriptionInputField.text = option.description.text;
            ownerInputField.text = option.productOwner.text;
            priceInputField.text = option.price.text;
            quantityInputField.text = option.GetSticker().quantity.ToString();
        }
    }

    public void OnSaveButtonClick()
    {
        //Remove all stickers in sticker list
        while (lastSavedStickers.Count > 0)
        {
            lastSavedStickers.RemoveAt(0);
        }

        //Put stickers from content scroll in lastSavedStickers list
        for(int i = 0; i < contentScroll.GetContentComponents().Length; i++)
        {
            lastSavedStickers.Add(new Sticker(contentScroll.GetContentComponents()[i].GetComponent<QROption>().GetSticker()));
        }

        OnDetailsChanged();
    }

    public void OnCloseEditPagePanel()
    {
        StickerPage returnPage = new StickerPage(lastSavedStickers[0].template);

        foreach (Sticker sticker in lastSavedStickers)
        {
            returnPage.AddSticker(sticker);
        }

        stickerDetailMenu.SetStickerPage(returnPage);
    }

    public void OnDescriptionChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
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

    public void OnQuantityChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            option.SetNumberOfStickers(int.Parse(quantityInputField.text));
        }
        OnDetailsChanged();
    }

    private void OnDetailsChanged()
    {
        //Check if any fields on any of the stickers are different than what is saved on the StickerPage.
        bool isPageDirty = false;
        for(int i = 0; i < lastSavedStickers.Count; i++)
        {
            Sticker lastSavedSticker = lastSavedStickers[i];
            Sticker currentSticker = contentScroll.GetContentComponents()[i].GetComponent<QROption>().GetSticker();

            if(lastSavedSticker.itemDescription != currentSticker.itemDescription)
            {
                isPageDirty = true;
            } 
            else if (lastSavedSticker.owner != currentSticker.owner)
            {
                isPageDirty = true;
            }
            else if (lastSavedSticker.price != currentSticker.price)
            {
                isPageDirty = true;
            }
            else if (lastSavedSticker.quantity != currentSticker.quantity)
            {
                isPageDirty = true;
            }
        }

        //If any stickers are different from what is on the StickerPage, enable the save button
        saveButton.interactable = isPageDirty;
    }

    public void OnQuantityAddButtonClick()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantityInputField.text) ? quantityInputField.text : "0");
        if (qty < 999999)
        {
            quantityInputField.text = (qty + 1).ToString();
        }
        else
        {
            quantityInputField.text = "999999";
        }
    }

    public void OnQuantityMinusButtonClick()
    {
        int qty = int.Parse(!string.IsNullOrEmpty(quantityInputField.text) ? quantityInputField.text : "0");
        if (qty > 0)
        {
            quantityInputField.text = (qty - 1).ToString();
        }
        else
        {
            quantityInputField.text = "0";
        }
    }
}
