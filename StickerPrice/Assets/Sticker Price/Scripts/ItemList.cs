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
	private List<ItemRow> itemList; 

	// Use this for initialization
	void Start () {
		scrollRect = this.GetComponent<ScrollRect> ();
		viewport = (RectTransform)this.transform.Find ("Viewport");
		scrollPanel = (RectTransform)viewport.transform.Find ("ContentPanel");
		topOfListPos = scrollPanel.localPosition.y;
		itemList = new List<ItemRow>(GetComponentsInChildren<ItemRow> ());
	}

	public void OnValueChange() {
		float verticalVelocity = Mathf.Abs (scrollRect.velocity.y);

		if (verticalVelocity < 2f) {
			scrollRect.velocity = Vector2.zero;
		}

		if (scrollPanel.transform.localPosition.y < topOfListPos) {
			float newY = Mathf.Lerp (scrollPanel.transform.localPosition.y,
							 topOfListPos,
				             Time.deltaTime * 5f);

			Vector2 newPosition = new Vector2 (scrollPanel.transform.localPosition.x, newY);
			scrollPanel.transform.localPosition = newPosition;
		}

		resetAllRows ();

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

		resetAllRows ();
	}
}
