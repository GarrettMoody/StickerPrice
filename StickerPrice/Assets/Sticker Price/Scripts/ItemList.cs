using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[Serializable]
public class ItemListData
{
    [HideInInspector] public float priceTotal = 0f;
    [HideInInspector] public float priceSubtotal = 0f;
    [HideInInspector] public float taxTotal = 0f;
    [HideInInspector] public float discount = 0f;
    [HideInInspector] public int itemTotal = 0;
}

public class ItemList : ContentList
{ 

    [Header("ItemList Variables")]

    [SerializeField] private Text priceTotalText;
    [SerializeField] private Text itemsTotalText;
    [SerializeField] private Text priceSubtotalText;
    [SerializeField] private GameObject discountPanel;
    [SerializeField] private Text discountText;
    [SerializeField] private Text taxText;

    [SerializeField] private AdjustPanel priceAdjustPanel;
    [SerializeField] private ItemRow itemPrefab;
    public ItemListData itemListData;
    private const float TAX_AMOUNT = 0.05f;
	
	// Use this for initialization
	public override void Start ()
	{
        base.Start();
		CalculateTotals ();
	}

    public override void Awake()
    {
        base.Awake();
        CalculateTotals();
    }

    override public void RemoveRow(ContentRow row)
    {
        base.RemoveRow(row);
        CalculateTotals();
    }

    override public void RemoveAllRows () {
        base.RemoveAllRows();
        CalculateTotals();
    }

	public ItemRow AddItem ()
	{
        ItemRow newItem = Instantiate(itemPrefab, contentPanel.transform);
		newItem.transform.SetAsLastSibling ();
		contentList.Add (newItem); 
        ResetAllRows();
		CalculateTotals ();
		return newItem;
	}

    public ItemRow AddItem (ItemRow row) {
        ItemRow newRow = Instantiate(row, contentPanel.transform);
        newRow.transform.SetAsLastSibling();
        contentList.Add(newRow);
        ResetAllRows();
        CalculateTotals();
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
        itemsTotalText.text = itemListData.itemTotal.ToString () + " Items";
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
