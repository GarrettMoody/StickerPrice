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

	private float bottomOfListPos;
	private List<ItemRow> itemList;

	private bool isLerping = false;
	private float priceTotal = 0f;
	private int itemTotal = 0;

	private const float topOfListPos = -900f;
	// Use this for initialization
	void Start ()
	{
		CalculateItemsAndPrice ();
		itemList = new List<ItemRow> (contentPanel.GetComponentsInChildren<ItemRow> ());
		SetBottomOfListPos ();
	}


	public void Update ()
	{
		MoveListToValidRange ();
	}

	public void MoveListToValidRange ()
	{
		if (contentPanel.transform.localPosition.y < topOfListPos - 5f || (contentPanel.transform.localPosition.y > bottomOfListPos - 5f) && itemList.Count != 1) {
			isLerping = true;
		} else {
			isLerping = false;
		}

		if (!Input.GetMouseButton (0)) {
			if (contentPanel.transform.localPosition.y < topOfListPos - 5f) {//If you're at the top of the list, you can't scroll up. Lerp back down to top.
				float newY = Mathf.Lerp (contentPanel.transform.localPosition.y,
					             topOfListPos,
					             Time.deltaTime * 10f);

				Vector2 newPosition = new Vector2 (contentPanel.transform.localPosition.x, newY);
				contentPanel.transform.localPosition = newPosition;
			}

			if (contentPanel.transform.localPosition.y > bottomOfListPos - 5f) {//If you're at the bottom of the list, you can't scroll down. Lerp back up to bottom.
				float newY = Mathf.Lerp (contentPanel.transform.localPosition.y,
					             bottomOfListPos,
					             Time.deltaTime * 10f);

				Vector2 newPosition = new Vector2 (contentPanel.transform.localPosition.x, newY);
				contentPanel.transform.localPosition = newPosition;
			}
		}
	}

	public void OnValueChange ()
	{
		if (Mathf.Abs (scrollRect.velocity.y) < 10f) {
			scrollRect.StopMovement ();
		}

		if (!Input.GetMouseButton (0)) {
			MoveListToValidRange ();
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
		RedrawList ();
		CalculateItemsAndPrice ();
	}

	public ItemRow AddItem ()
	{

		ItemRow newItem = Instantiate (itemPrefab, contentPanel.transform);
		newItem.GetComponent<LayoutElement> ().ignoreLayout = false;
		newItem.transform.SetParent (contentPanel.transform);
		newItem.transform.SetAsLastSibling ();
		itemList.Add (newItem);

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
        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                row.GetComponent<LayoutElement>().ignoreLayout = false;
            }
        }

        Canvas.ForceUpdateCanvases ();

        if (itemList != null)
        {
            foreach (ItemRow row in itemList)
            {
                row.GetComponent<LayoutElement>().ignoreLayout = true;
            }
        }

        SetBottomOfListPos ();
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
        //if (itemList != null) {
        //	SetItemTotal (itemList.Count);
        //	if (itemList.Count > 0) {
        //		SetPriceTotal (0);
        //		foreach (ItemRow row in itemList) {
        //			SetPriceTotal (priceTotal + row.GetItemPrice ());
        //		}
        //	} else {
        //		SetPriceTotal (0);
        //	}
        //} else {
        //	SetPriceTotal (0);
        //	SetItemTotal (0);
        //}
    }

    public void SetBottomOfListPos ()
	{
		//Takes number of items * the height of each item, will get you the bottom of the bottom item. We want the top of the bottom item so the 
		//panel always shows at least one item. So we take the number of items - 1. 
		bottomOfListPos = (itemList.Count - 1) * itemPrefab.GetComponent<RectTransform> ().rect.height + topOfListPos;
		OnValueChange ();
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
