﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemList : ContentList
{ 
    [Header("ItemList Variables")]

    public Text priceTotalText;
    public TextMeshProUGUI itemsTotalText;
    public Text priceSubtotalText;
    public GameObject discountPanel;
    public Text discountText;
    public Text taxText;
    public Button checkoutButton;

    public AdjustPanel priceAdjustPanel;
    public ItemRow itemPrefab;
    public ItemListData itemListData;
    private const float TAX_AMOUNT = 0.00f;
	
	// Use this for initialization
	public override void Start ()
	{
        base.Start();
		CalculateTotals ();
        SetCheckoutButtonInteraction();
	}

    public override void Awake()
    {
        base.Awake();
        CalculateTotals();
        SetCheckoutButtonInteraction();
    }

    public void RemoveRow(ItemRow row)
    {
        itemListData.itemRowDataListContainer.itemRowDataList.Remove(row.itemRowData);
        ContentRow contentRow = row;
        base.RemoveRow(contentRow);
        CalculateTotals();
        SetCheckoutButtonInteraction();
    }

    override public void RemoveAllRows () {
        itemListData.itemRowDataListContainer.itemRowDataList.Clear();
        base.RemoveAllRows();
        CalculateTotals();
        SetCheckoutButtonInteraction();
    }

	public ItemRow AddItem ()
	{
        ItemRow newItem = Instantiate(itemPrefab, contentPanel.transform);
		newItem.transform.SetAsLastSibling ();
		contentList.Add (newItem);
        itemListData.itemRowDataListContainer.itemRowDataList.Add(newItem.itemRowData);
        ResetAllRows();
		CalculateTotals ();
        SetCheckoutButtonInteraction();
		return newItem;
	}

    public ItemRow AddItem (ItemRow row) {
        ItemRow newRow = Instantiate(row, contentPanel.transform);
        newRow.transform.SetAsLastSibling();
        contentList.Add(newRow);
        itemListData.itemRowDataListContainer.itemRowDataList.Add(row.itemRowData);
        ResetAllRows();
        CalculateTotals();
        SetCheckoutButtonInteraction();
        return newRow;
    }

	public void CalculateTotals ()
	{
        //Reset totals
        SetItemTotal(0);
        SetPriceSubtotal(0);

        //For each item in the list add to item total and subtotal
        if (base.contentList != null)
        {
            foreach (ItemRow row in base.contentList)
            {
                SetItemTotal(GetItemTotal() + row.GetQuantity());
                SetPriceSubtotal(GetPriceSubtotal() + (row.GetQuantity() * row.GetItemPrice()));
            }
        }
        SetTaxTotal((GetPriceSubtotal() - GetDiscountPrice()) * TAX_AMOUNT);
        SetPriceTotal(GetPriceSubtotal() + GetTaxTotal() - GetDiscountPrice());
    }

    public void SetCheckoutButtonInteraction ()
    {
        if(checkoutButton != null) //Checkout Button is optional
        {
            if (contentList.Count == 0) //If no items are in the list, checkout button is disabled; nothing to checkout. 
            {
                checkoutButton.interactable = false;
            }
            else
            {
                checkoutButton.interactable = true;
            }
        }
    }

    public void UpdatePriceSubtotalText ()
	{
        priceSubtotalText.text = itemListData.priceSubtotal.ToString ("C2");
	}

    public float GetPriceSubtotal ()
	{
		return itemListData.priceSubtotal;
	}

    public void SetPriceSubtotal (float value)
	{
        itemListData.priceSubtotal = value;
        UpdatePriceSubtotalText();
	}

  
    public float GetPriceTotal() {
        return itemListData.priceTotal;
    }

    public void SetPriceTotal(float value) {
        itemListData.priceTotal = value;
        UpdatePriceTotalText();
    }

    public void UpdatePriceTotalText() {
        if(priceTotalText != null) {
            priceTotalText.text = itemListData.priceTotal.ToString("C2");
        }
    }

    public int GetItemTotal ()
	{
		return itemListData.itemTotal;
	}

    public void SetItemTotal (int value)
	{
        itemListData.itemTotal = value;
		UpdateItemTotalText ();
	}

    public float GetTaxTotal() {
        return itemListData.taxTotal;
    }

    public void SetTaxTotal(float tax) {
        itemListData.taxTotal = tax;
        UpdateTaxText();
    }

    public void UpdateTaxText() {
        if(taxText != null) {
            taxText.text = itemListData.taxTotal.ToString("C2");
        }
    }

    public void UpdateItemTotalText ()
	{
        itemsTotalText.text = itemListData.itemTotal.ToString();
	}

    public void DiscountButtonOnClickListener() {
        this.gameObject.SetActive(false);
        priceAdjustPanel.OpenAdjustPanel(this);
    }

    public float GetDiscountPrice() {
        return itemListData.discount;
    }

    public void SetDiscountPrice(float value) {
        itemListData.discount = value;
        UpdateDiscountPriceText();
       CalculateTotals();
    }

    public void UpdateDiscountPriceText() {
        if(itemListData.discount != 0) { //If there is a discounted price
            discountPanel.SetActive(true); //Display the discount
        } else {
            discountPanel.SetActive(false);
        }
        discountText.text = itemListData.discount.ToString("C2");
    }

    public void OpenPriceAdjustPanel(ItemRow row) {
        priceAdjustPanel.OpenAdjustPanel(row);
    }

    public void OpenPriceAdjustPanel(ItemList list) {
        priceAdjustPanel.OpenAdjustPanel(list);
    }

}
