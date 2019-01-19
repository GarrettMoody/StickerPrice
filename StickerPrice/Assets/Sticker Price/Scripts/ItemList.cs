using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemList : ContentList
{

    [Header("ItemList Variables")]
	public ItemRow itemPrefab;
	public Text priceTotalText;
	public Text itemsTotalText;
    public Text priceSubtotalText;
    public GameObject discountPanel;
    public Text discountText;
    public Text taxText;

    public AdjustPanel priceAdjustPanel;

    private const float TAX_AMOUNT = 0.05f;
	
    private float priceTotal = 0f;
    private float priceSubtotal = 0f;
    private float taxTotal = 0f;
    private float discount = 0f;
	private int itemTotal = 0;

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
        priceSubtotalText.text = priceSubtotal.ToString ("C2");
	}

    public float GetPriceSubtotal ()
	{
		return priceSubtotal;
	}

    public void SetPriceSubtotal (float value)
	{
		priceSubtotal = value;
        UpdatePriceSubtotalText();
	}

  
    public float GetPriceTotal() {
        return priceTotal;
    }

    public void SetPriceTotal(float value) {
        priceTotal = value;
        UpdatePriceTotalText();
    }

    public void UpdatePriceTotalText() {
        if(priceTotalText != null) {
            priceTotalText.text = priceTotal.ToString("C2");
        }
    }

    public int GetItemTotal ()
	{
		return itemTotal;
	}

    public void SetItemTotal (int value)
	{
		itemTotal = value;
		UpdateItemTotalText ();
	}

    public float GetTaxTotal() {
        return taxTotal;
    }

    public void SetTaxTotal(float tax) {
        taxTotal = tax;
        UpdateTaxText();
    }

    public void UpdateTaxText() {
        if(taxText != null) {
            taxText.text = taxTotal.ToString("C2");
        }
    }

    public void UpdateItemTotalText ()
	{
		itemsTotalText.text = itemTotal.ToString () + " Items";
	}

    public void DiscountButtonOnClickListener() {
        this.gameObject.SetActive(false);
        priceAdjustPanel.OpenAdjustPanel(this);
    }

    public float GetDiscountPrice() {
        return discount;
    }

    public void SetDiscountPrice(float value) {
        discount = value;
        UpdateDiscountPriceText();
       CalculateTotals();
    }

    public void UpdateDiscountPriceText() {
        if(discount != 0) { //If there is a discounted price
            discountPanel.SetActive(true); //Display the discount
        } else {
            discountPanel.SetActive(false);
        }
        discountText.text = discount.ToString("C2");
    }

    public void OpenPriceAdjustPanel(ItemRow row) {
        priceAdjustPanel.OpenAdjustPanel(row);
    }

    public void OpenPriceAdjustPanel(ItemList list) {
        priceAdjustPanel.OpenAdjustPanel(list);
    }

}
