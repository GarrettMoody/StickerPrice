using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemList : MonoBehaviour
{

	public ItemRow itemPrefab;
	public Text priceTotalText;
	public Text itemsTotalText;
    public Text priceSubtotalText;
    public GameObject discountPanel;
    public Text discountText;
    public Text taxText;
    public ScrollRect scrollRect;
    public RectTransform viewport;
    public RectTransform contentPanel;
    public AdjustPanel priceAdjustPanel;

    private const float TAX_AMOUNT = 0.05f;
	private List<ItemRow> itemList;
	private bool isLerping = false;
    private float priceTotal = 0f;
    private float priceSubtotal = 0f;
    private float taxTotal = 0f;
    private float discount = 0f;
	private int itemTotal = 0;

	// Use this for initialization
	void Start ()
	{
		CalculateTotals ();
		itemList = new List<ItemRow> (contentPanel.GetComponentsInChildren<ItemRow> ());
	}

    private void Awake()
    {
        CalculateTotals();
        itemList = new List<ItemRow>(contentPanel.GetComponentsInChildren<ItemRow>());
    }

	public void OnValueChange ()
	{
		if (Mathf.Abs (scrollRect.velocity.y) < 10f) {
			scrollRect.StopMovement ();
		}

		if (!Input.GetMouseButton (0)) {
			ResetAllRows ();
		}
	}

	public void StopScrolling ()
	{
		scrollRect.StopMovement ();
	}

	public void RemoveItem (ItemRow item)
	{
		itemList.Remove (item);
		Destroy (item.gameObject);
        //contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x,
                                             //contentPanel.sizeDelta.y - itemPrefab.GetComponent<RectTransform>().rect.height);

        RedrawList();
		CalculateTotals ();
	}

    public void RemoveAllItems () {
        while(itemList.Count > 0) {
            RemoveItem(itemList[0]);
        }
        RedrawList();
        CalculateTotals();
    }
    
	public ItemRow AddItem ()
	{

		ItemRow newItem = Instantiate (itemPrefab, contentPanel.transform);
		//newItem.GetComponent<LayoutElement> ().ignoreLayout = false;
		newItem.transform.SetParent (contentPanel.transform);
		newItem.transform.SetAsLastSibling ();
		itemList.Add (newItem);

        //contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, 
                                             //contentPanel.sizeDelta.y + itemPrefab.GetComponent<RectTransform>().rect.height);

		RedrawList ();
		CalculateTotals ();
		return newItem;
	}

    public ItemRow AddItem (ItemRow row) {
        ItemRow newItem = Instantiate(row, contentPanel.transform);
        float newPrice = newItem.GetItemPrice();

        itemList.Add(newItem);
        RedrawList();
        CalculateTotals();
        return newItem;
    }

	public void ResetAllRows ()
	{
        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                row.ResetRow();
            }
        }
    }

	public void ResetOtherRows (ItemRow sourceRow)
	{
        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                if (row != sourceRow)
                {
                    row.ResetRow();
                }
            }
        }
    }

    public void RedrawList ()
	{
        //if (itemList != null)
        //{
        //    foreach (ItemRow row in itemList)
        //    {
        //        row.GetComponent<LayoutElement>().ignoreLayout = false;
        //    }
        //}

        //Canvas.ForceUpdateCanvases ();

        //if (itemList != null)
        //{
        //    foreach (ItemRow row in itemList)
        //    {
        //        row.GetComponent<LayoutElement>().ignoreLayout = true;
        //    }
        //}

		ResetAllRows ();
	}

	public void CalculateTotals ()
	{
        //Reset totals
        SetItemTotal(0);
        SetPriceSubtotal(0);

        //For each item in the list add to item total and subtotal
        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                SetItemTotal(GetItemTotal() + row.GetQuantity());
                SetPriceSubtotal(GetPriceSubtotal() + (row.GetQuantity() * row.GetItemPrice()));
            }
        }
        SetTaxTotal((GetPriceSubtotal() - GetDiscountPrice()) * TAX_AMOUNT);
        SetPriceTotal(GetPriceSubtotal() + GetTaxTotal() - GetDiscountPrice());
    }

    public void SetIsLerping (bool value)
	{
		isLerping = value;
	}

	public bool GetIsLerping ()
	{
		return isLerping;
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

    public List<ItemRow> GetItemRows() {
        return itemList;
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
