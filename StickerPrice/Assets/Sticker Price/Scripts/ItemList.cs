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

	private ScrollRect scrollRect;
	private RectTransform viewport;
	private RectTransform scrollPanel;
	private float bottomOfListPos;
	private List<ItemRow> itemList;

	private bool isLerping = false;
	private float priceTotal = 0f;
	private int itemTotal = 0;

	private const float topOfListPos = -900f;
	// Use this for initialization
	void Start ()
	{
		calculateItemsAndPrice ();
		scrollRect = this.GetComponent<ScrollRect> ();
		viewport = (RectTransform)this.transform.Find ("Viewport");
		scrollPanel = (RectTransform)viewport.transform.Find ("ContentPanel");
		itemList = new List<ItemRow> (scrollPanel.GetComponentsInChildren<ItemRow> ());
		setBottomOfListPos ();
	}


	public void Update ()
	{
		MoveListToValidRange ();
	}

	public void MoveListToValidRange ()
	{
		//Debug.Log ("Lerping:" + isLerping + " Y:" + scrollPanel.transform.localPosition.y + " topPos:" + topOfListPos + " bottomPos:" + bottomOfListPos);
		if (scrollPanel.transform.localPosition.y < topOfListPos - 5f || (scrollPanel.transform.localPosition.y > bottomOfListPos - 5f) && itemList.Count != 1) {
			isLerping = true;
		} else {
			isLerping = false;
		}

		if (!Input.GetMouseButton (0)) {
			if (scrollPanel.transform.localPosition.y < topOfListPos - 5f) {//If you're at the top of the list, you can't scroll up. Lerp back down to top.
				float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
					             topOfListPos,
					             Time.deltaTime * 10f);

				Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
				scrollPanel.transform.localPosition = newPosition;
			}

			if (scrollPanel.transform.localPosition.y > bottomOfListPos - 5f) {//If you're at the bottom of the list, you can't scroll down. Lerp back up to bottom.
				float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
					             bottomOfListPos,
					             Time.deltaTime * 10f);

				Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
				scrollPanel.transform.localPosition = newPosition;
			}
		}
	}

	public void OnValueChange ()
	{
		//Debug.Log (scrollRect.velocity.y);
		if (Mathf.Abs (scrollRect.velocity.y) < 10f) {
			scrollRect.StopMovement ();
		}

		if (!Input.GetMouseButton (0)) {
			MoveListToValidRange ();
			resetAllRows ();
		}
	}

	public void stopScrolling ()
	{
		scrollRect.StopMovement ();
	}

	public void removeItem (ItemRow item)
	{
		itemList.Remove (item);
		Destroy (item.gameObject);
		redrawList ();
		calculateItemsAndPrice ();
	}

	public ItemRow addItem ()
	{

		ItemRow newItem = Instantiate (itemPrefab, scrollPanel.transform);
		newItem.GetComponent<LayoutElement> ().ignoreLayout = false;
		newItem.transform.SetParent (scrollPanel.transform);
		newItem.transform.SetAsLastSibling ();
		itemList.Add (newItem);

		redrawList ();
		calculateItemsAndPrice ();
		return newItem;
	}

	public void resetAllRows ()
	{
		foreach (ItemRow row in itemList) {
			row.ResetRow ();
		}
	}

	public void resetOtherRows (ItemRow sourceRow)
	{
		foreach (ItemRow row in itemList) {
			if (row != sourceRow) {
				row.ResetRow ();
			}
		}
	}

	public void redrawList ()
	{
		foreach (ItemRow row in itemList) {
			row.GetComponent<LayoutElement> ().ignoreLayout = false;
		}

		Canvas.ForceUpdateCanvases ();

		foreach (ItemRow row in itemList) {
			row.GetComponent<LayoutElement> ().ignoreLayout = true;
		}

		setBottomOfListPos ();
		resetAllRows ();
	}

	public void calculateItemsAndPrice ()
	{
		if (itemList != null) {
			setItemTotal (itemList.Count);
			if (itemList.Count > 0) {
				foreach (ItemRow row in itemList) {
					setPriceTotal (priceTotal + row.getItemPrice ());
				}
			} else {
				setPriceTotal (0);
			}
		} else {
			setPriceTotal (0);
			setItemTotal (0);
		}
	}

	public void setBottomOfListPos ()
	{
		//Takes number of items * the height of each item, will get you the bottom of the bottom item. We want the top of the bottom item so the 
		//panel always shows at least one item. So we take the number of items - 1. 
		bottomOfListPos = (itemList.Count - 1) * itemPrefab.GetComponent<RectTransform> ().rect.height + topOfListPos;
		OnValueChange ();
	}

	public void setIsLerping (bool value)
	{
		isLerping = value;
	}

	public bool getIsLerping ()
	{
		return isLerping;
	}

	public void updatePriceTotalText ()
	{
		priceTotalText.text = "Total: " + priceTotal.ToString ("C");
	}

	public float getPriceTotal ()
	{
		return priceTotal;
	}

	public void setPriceTotal (float value)
	{
		priceTotal = value;
		updatePriceTotalText ();
	}

	public int getItemTotal ()
	{
		return itemTotal;
	}

	public void setItemTotal (int value)
	{
		itemTotal = value;
		updateItemTotalText ();
	}

	public void updateItemTotalText ()
	{
		itemsTotalText.text = itemTotal.ToString () + " Items";
	}
}
