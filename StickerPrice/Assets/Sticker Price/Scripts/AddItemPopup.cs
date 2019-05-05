using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class AddItemPopup: MonoBehaviour
{
    public GameObject popupPanel;
    public ScanPanel scanPanel;
    public InputField itemDescription;
    public InputField productOwner;
    public InputField price;

    public void SaveButtonOnClickListener()
    {
        Sticker sticker = new Sticker();
        float itemPrice = float.Parse(price.text, NumberStyles.Currency);

        sticker.itemDescription = itemDescription.text;
        sticker.owner = productOwner.text;
        sticker.price = itemPrice.ToString();

        scanPanel.AddItem(sticker);

        this.gameObject.SetActive(false);
        popupPanel.gameObject.SetActive(false);
    }

    public void CancelButtonOnClickListener()
    {
        this.gameObject.SetActive(false);
        popupPanel.gameObject.SetActive(false);
    }
}