using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

	//Private Variables
    [SerializeField, HideInInspector] private string itemDescription = "New Item";
    [SerializeField, HideInInspector] private float itemOriginalPrice = 10;
    [SerializeField, HideInInspector] private float itemPrice = 10;
    [SerializeField, HideInInspector] private int quantity = 1;
    [SerializeField, HideInInspector] private string productOwner;
    [SerializeField, HideInInspector] private ItemList parentItemList;
	private readonly RectTransform itemRectTransform;
	private readonly RectTransform contentRectTransform;

    [SerializeField, HideInInspector] private string scanString;

    public override void Awake()
    {
        base.Awake();
        parentItemList = GetComponentInParent<ItemList>();
        UpdatePriceText();
    }

    public void OnPlusButtonClick() {
        SetQuantity(quantity + 1);
    }

    public void OnMinusButtonClick() {
        SetQuantity(quantity - 1);
    }

    public void DeleteButtonOnClickListener ()
    { 
        parentItemList.RemoveRow (this);
	}

    public void AdjustButtonOnClickListener()
    {
        ResetRow();
        parentItemList.OpenPriceAdjustPanel(this);
    }

	public void SetItemDescription (string description)
	{
		itemDescription = description;
		itemDescriptionText.text = itemDescription;
	}

	public string GetItemDescription ()
	{
		return itemDescription;
	}

	public void SetItemPrice (float price)
	{
		itemPrice = price;
        UpdatePriceText();
		parentItemList.CalculateTotals ();
	}

	public float GetItemPrice ()
	{
		return itemPrice;
	}

	public void SetItemOriginalPrice (float price)
	{
		itemOriginalPrice = price;
        UpdatePriceText();
	}

	public float GetItemOriginalPrice ()
	{
		return itemOriginalPrice;
	}

    public int GetQuantity() {
        return quantity;
    }

    public void SetQuantity(int value) {
        if (value >= 0 && value <= 99)
        {
            quantity = value;
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
        quantityInputField.text = quantity.ToString();
        parentItemList.CalculateTotals();
    }

    public void OnQuantityInputFieldValueChanged() {
        quantity = int.Parse(quantityInputField.text);
    }

    public string GetScanString () {
        return scanString;
    }

    public void SetScanString(string value) {
        scanString = value;
        Texture2D qrCode = QRCodeGenerator.CreateQRCode(scanString);
        QRCode.texture = qrCode;
    }

    public string GetProductOwner() {
        return productOwner;
    }

    public void SetProductOwner(string value) {
        productOwner = value;
        UpdateProductOwnerText();
    }

    public void UpdateProductOwnerText() {
        productOwnerText.text = productOwner;
    }

    public override void DefaultButtonOnClickListener()
    {
        DeleteButtonOnClickListener();
    }
}
