using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemList : MonoBehaviour {

	public ItemRow itemPrefab;

	private ScrollRect scrollRect;
	private RectTransform viewport;
	private RectTransform scrollPanel;
	private float topOfListPos;
	private float bottomOfListPos;
	private List<ItemRow> itemList; 

	// Use this for initialization
	void Start () {
		scrollRect = this.GetComponent<ScrollRect> ();
		viewport = (RectTransform)this.transform.Find ("Viewport");
		scrollPanel = (RectTransform)viewport.transform.Find ("ContentPanel");
		itemList = new List<ItemRow>(scrollPanel.GetComponentsInChildren<ItemRow>());
		topOfListPos = scrollPanel.localPosition.y;
		setBottomOfListPos ();
	}


	public void Update() {
		MoveListToValidRange ();
	}

	public void MoveListToValidRange() {
		if (!Input.GetMouseButton (0)) {
			if (scrollPanel.transform.localPosition.y < topOfListPos) {//If you're at the top of the list, you can't scroll up. Lerp back down to top.
				float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
					            topOfListPos,
					            Time.deltaTime * 5f);

				Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
				scrollPanel.transform.localPosition = newPosition;
			}

			if (scrollPanel.transform.localPosition.y > bottomOfListPos) {//If you're at the bottom of the list, you can't scroll down. Lerp back up to bottom.
				float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
					            bottomOfListPos,
					            Time.deltaTime * 5f);

				Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
				scrollPanel.transform.localPosition = newPosition;
			}
		}
	}
		
	public void OnValueChange() {
		//Debug.Log ("OnValueChange");
		if (Mathf.Abs(scrollRect.velocity.y) < 10f) {
			scrollRect.StopMovement();
		}

		if (!Input.GetMouseButton (0)) {
			MoveListToValidRange ();
			resetAllRows ();
		}


	}

	public void stopScrolling() {
		scrollRect.StopMovement ();
	}

	public void removeItem(ItemRow item) {
		itemList.Remove (item);
		Destroy (item.gameObject);
		redrawList ();
	}

	public ItemRow addItem() {

		ItemRow newItem = Instantiate (itemPrefab, scrollPanel.transform);
		newItem.GetComponent<LayoutElement> ().ignoreLayout = false;
		newItem.transform.SetParent (scrollPanel.transform);
		newItem.transform.SetAsLastSibling ();
		itemList.Add (newItem);

		redrawList ();

		return newItem;
	}

	public void resetAllRows() {
		foreach (ItemRow row in itemList) {
			row.ResetRow ();
		}
	}

	public void resetOtherRows(ItemRow sourceRow) {
		foreach (ItemRow row in itemList) {
			if (row != sourceRow) {
				row.ResetRow ();
			}
		}
	}

	public void redrawList() {
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

	public void setBottomOfListPos() {
		//Takes number of items * the height of each item, will get you the bottom of the bottom item. We want the top of the bottom item so the 
		//panel always shows at least one item. So we take the number of items - 1. 
		bottomOfListPos = (itemList.Count - 1) * itemPrefab.GetComponent<RectTransform> ().rect.height + topOfListPos;
		OnValueChange ();
	}
}
