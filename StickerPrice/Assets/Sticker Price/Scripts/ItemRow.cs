using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemRow : ContentRow
{
	//Public Variables
    [Header("Item Row Properties")]
	public Text itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Text adjustedPriceText;
    public Text productOwnerText;
    public Button plusButton;
    public Button minusButton;
    public InputField quantityInputField;
    public RectTransform horizontalContentPanel;
    public RawImage QRCode;
    public ItemRowData itemRowData;
	//Private Variables
   
    private ItemList parentItemList;
	private readonly RectTransform itemRectTransform;
	private readonly RectTransform contentRectTransform;

    public override void Awake()
    {
        base.Awake();
        parentItemList = GetComponentInParent<ItemList>();
        UpdatePriceText();
    }

    public void OnPlusButtonClick() {
        SetQuantity(itemRowData.quantity + 1);
    }

    public void OnMinusButtonClick() {
        SetQuantity(itemRowData.quantity - 1);
    }

    public void OnDeleteButtonClick ()
    { 
        parentItemList.RemoveRow (this);
	}

    public void OnAdjustButtonClick()
    {
        ResetRow();
        parentItemList.OpenPriceAdjustPanel(this);
    }

	public void SetItemDescription (string description)
	{
        itemRowData.itemDescription = description;
		itemDescriptionText.text = itemRowData.itemDescription;
	}

	public string GetItemDescription ()
	{
		return itemRowData.itemDescription;
	}

	public void SetItemPrice (float price)
	{
        itemRowData.itemPrice = price;
        UpdatePriceText();
		parentItemList.CalculateTotals ();
	}

	public float GetItemPrice ()
	{
		return itemRowData.itemPrice;
	}

	public void SetItemOriginalPrice (float price)
	{
        itemRowData.itemOriginalPrice = price;
        UpdatePriceText();
	}

	public float GetItemOriginalPrice ()
	{
		return itemRowData.itemOriginalPrice;
	}

    public int GetQuantity() {
        return itemRowData.quantity;
    }

    public void SetQuantity(int value) {
        if (value >= 0 && value <= 99)
        {
            itemRowData.quantity = value;
            UpdateQuantity();
        }
    }

    public void UpdatePriceText() {
        //Was the price adjusted for this item?
        if(GetItemPrice() != GetItemOriginalPrice()) {
            //Price is adjusted. Strikethrough original price and display adjusted price
            itemPriceText.SetText(GetItemOriginalPrice().ToString("C2"));
            itemPriceText.fontStyle = FontStyles.Strikethrough;
            adjustedPriceText.text = GetItemPrice().ToString("C2");
            adjustedPriceText.gameObject.SetActive(true);
        } else {
            //Price is original price. Hide adjusted price
            itemPriceText.SetText(GetItemPrice().ToString("C2"));
            itemPriceText.fontStyle = FontStyles.Normal;
            adjustedPriceText.gameObject.SetActive(false);
        }
    }

    public void UpdateQuantity() {
        quantityInputField.text = itemRowData.quantity.ToString();
        parentItemList.CalculateTotals();
    }

    public void OnQuantityInputFieldValueChanged() {
        itemRowData.quantity = int.Parse(quantityInputField.text);
    }

    public string GetScanString () {
        return itemRowData.scanString;
    }

    public void SetScanString(string value) {
        itemRowData.scanString = value;
        Texture2D qrCode = StickerQRCode.CreateQRCode(itemRowData.scanString);
        QRCode.texture = qrCode;
    }

    public string GetProductOwner() {
        return itemRowData.productOwner;
    }

    public void SetProductOwner(string value) {
        itemRowData.productOwner = value;
        UpdateProductOwnerText();
    }

    public void UpdateProductOwnerText() {
        productOwnerText.text = itemRowData.productOwner;
    }

    public override void OnDefaultButtonClick()
    {
        OnDeleteButtonClick();
    }
}
