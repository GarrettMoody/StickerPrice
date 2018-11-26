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
    public ScrollRect scrollRect;
    public RectTransform viewport;
    public RectTransform contentPanel;

	private List<ItemRow> itemList;

	private bool isLerping = false;
	private float priceTotal = 0f;
	private int itemTotal = 0;

	// Use this for initialization
	void Start ()
	{
		CalculateItemsAndPrice ();
		itemList = new List<ItemRow> (contentPanel.GetComponentsInChildren<ItemRow> ());
	}


	public void Update ()
	{

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
		CalculateItemsAndPrice ();
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
		CalculateItemsAndPrice ();
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

	public void CalculateItemsAndPrice ()
	{
        //Reset totals
        SetItemTotal(0);
        SetPriceTotal(0);

        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                SetItemTotal(GetItemTotal() + row.GetQuantity());
                SetPriceTotal(GetPriceTotal() + (row.GetQuantity() * row.GetItemPrice()));
            }
        }
    }

    public void SetIsLerping (bool value)
	{
		isLerping = value;
	}

	public bool GetIsLerping ()
	{
		return isLerping;
	}

    public void UpdatePriceTotalText ()
	{
		priceTotalText.text = "Total: " + priceTotal.ToString ("C");
	}

    public float GetPriceTotal ()
	{
		return priceTotal;
	}

    public void SetPriceTotal (float value)
	{
		priceTotal = value;
		UpdatePriceTotalText ();
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

    public void UpdateItemTotalText ()
	{
		itemsTotalText.text = itemTotal.ToString () + " Items";
	}
}
