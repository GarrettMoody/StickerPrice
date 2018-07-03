using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemRow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler {


	private Vector2 positionOffset;
	private RectTransform canvasRectTransform;
	private RectTransform itemRectTransform;
	private bool pointerDown = false;
	private bool lerpToButtons = false;
	private bool lerpToReset = false;
	private bool lerpToDelete = false;
	private bool lerpToPosition = false;
	private float buttonOffset = 400;
	private float deleteOffset = 800;
	private float fullDeleteOffset = 1200;
	private ItemList parentList; 
	private float dragX;

	void Update() {
		if (!pointerDown) {
			if (lerpToButtons) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, -buttonOffset, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == -buttonOffset) {
					lerpToButtons = false;
				}
			} else if (lerpToReset) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, 0f, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == 0) {
					lerpToReset = false;
				}
			} else if (lerpToDelete) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, -fullDeleteOffset, Time.deltaTime * 8);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == -fullDeleteOffset) {
					lerpToDelete = false;
				}
			} else if (lerpToPosition) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, dragX, Time.deltaTime * 8);
				if (newX > 0) {
					newX = 0;
				}
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == dragX) {
					lerpToPosition = false;
				}
			}
		}
	}

	void Awake() {
		Canvas canvas = this.GetComponentInParent<Canvas> ();
		canvasRectTransform = (RectTransform)canvas.transform;
		itemRectTransform = (RectTransform) this.transform;
		lerpToReset = false;
		lerpToButtons = false;
		lerpToDelete = false;
		lerpToPosition = false;
		parentList = GetComponentInParent<ItemList> ();
	}

	public void OnPointerDown(PointerEventData data) {
		lerpToReset = false;
		lerpToButtons = false;
		lerpToDelete = false;
		lerpToPosition = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (itemRectTransform, data.position, data.pressEventCamera, out positionOffset);
		parentList.resetOtherRows (this);
	}

	public void OnPointerUp(PointerEventData data) {
		int localX = Mathf.RoundToInt (itemRectTransform.localPosition.x);
		if ((localX < -buttonOffset && localX >= -deleteOffset) || (lerpToPosition && localX <= -deleteOffset)) {
			lerpToPosition = false;
			lerpToButtons = true;
		} else if (localX >= -buttonOffset) {
			lerpToButtons = false;
			lerpToPosition = false;
			lerpToReset = true;
		} else if (localX < -deleteOffset && !lerpToPosition) {
			DeleteButtonOnClickListener ();
		} 
	}

	public void OnDrag(PointerEventData data) {
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRectTransform, data.position, data.pressEventCamera, out localPointerPosition)) {
			float newX = localPointerPosition.x + (itemRectTransform.rect.width / 2) - positionOffset.x;
			dragX = newX;
			//Debug.Log ("newX: " + newX + " localX: " + itemRectTransform.localPosition.x + " Delete: " + lerpToDelete + " Position: " + lerpToPosition);
			if (newX > 0) { //trying to swipe right
				newX = 0;
			}
			if (newX < -deleteOffset) { //activate delete
				lerpToDelete = true;
				lerpToPosition = false;
			} else if (newX >= -deleteOffset && Mathf.RoundToInt(itemRectTransform.localPosition.x) < -deleteOffset) {
				lerpToDelete = false; 
				lerpToPosition = true;
			} else {
				itemRectTransform.localPosition = new Vector2 (newX, this.transform.localPosition.y);
			}
		}
	}

	public void OnPointerClick(PointerEventData data) {
		if (Mathf.RoundToInt(itemRectTransform.localPosition.x) > (-buttonOffset - 5)) {
			parentList.resetAllRows ();
		}
	}

	public void DeleteButtonOnClickListener() {
		parentList.removeItem (this);
	}

	public void ResetRow() {
		lerpToReset = true;
	}
}
