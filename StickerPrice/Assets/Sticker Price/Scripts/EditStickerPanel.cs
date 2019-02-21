using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditStickerPanel : MonoBehaviour
{
    public ContentScroll contentScroll;
    public InputField descriptionInputField;
    public InputField ownerInputField;
    public InputField imageInputField;
    public InputField priceInputField;
    public QROption qrOptionPrefab;

    void OnEnable()
    {
        contentScroll.OnSelectionChange += UpdateDetailsPanel;
    }

    void OnDisable()
    {
        contentScroll.OnSelectionChange -= UpdateDetailsPanel;
    }

    public void OpenEditStickerPanel(Sticker loadSticker)
    {
        this.gameObject.SetActive(true);
        StickerData stickerData = new StickerData();

        //Remove all QROptions in the list
        foreach (Transform child in contentScroll.content.transform)
        {
            Destroy(child.gameObject);
        }

        //Create a QROption for each sticker in the JSON file
        int i = 0;
        foreach (Sticker sticker in stickerData.GetAllStickers())
        {
            QROption newOption = Instantiate(qrOptionPrefab, contentScroll.content.transform);
            newOption.SetPrice(sticker.price);
            newOption.SetDescription(sticker.itemDescription);
            newOption.SetProductionOwner(sticker.owner);
            newOption.transform.localPosition = new Vector3(575 + (i * 150), newOption.transform.localPosition.y);
            i++;
        }

        RectTransform rectTransform = contentScroll.content.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1500 + (i * 100), rectTransform.rect.height);
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

    public void OnDescriptionChange()
    { 
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if(option != null)
        {
            option.description.text = descriptionInputField.text;
        }
    }

    public void OnProductOwnerChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            option.productOwner.text = ownerInputField.text;
        }
    }

    public void OnPriceChange()
    {
        QROption option = contentScroll.GetSelectedComponent().GetComponent<QROption>();
        if (option != null)
        {
            option.price.text = priceInputField.text;
        }
    }
}
