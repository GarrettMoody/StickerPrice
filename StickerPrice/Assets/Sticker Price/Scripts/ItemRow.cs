﻿using System.Collections;
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
	private float buttonOffset = 400;

	void Update() {
		if (!pointerDown) {
			if (lerpToButtons) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, -buttonOffset, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt(itemRectTransform.localPosition.x) == -buttonOffset) {
					lerpToButtons = false;
				}
			} else if (lerpToReset) {
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, 0f, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt(itemRectTransform.localPosition.x) == 0) {
					lerpToReset = false;
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
	}

	public void OnPointerDown(PointerEventData data) {
		lerpToReset = false;
		lerpToButtons = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (itemRectTransform, data.position, data.pressEventCamera, out positionOffset);
	}

	public void OnPointerUp(PointerEventData data) {
		if (Mathf.RoundToInt(itemRectTransform.localPosition.x) < -buttonOffset) {
			lerpToButtons = true;
		} else if (Mathf.RoundToInt(itemRectTransform.localPosition.x) >= -buttonOffset) {
			lerpToReset = true;
		}
	}

	public void OnDrag(PointerEventData data) {
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRectTransform, data.position, data.pressEventCamera, out localPointerPosition)) {
			float newX = localPointerPosition.x + (itemRectTransform.rect.width / 2) - positionOffset.x;
			Debug.Log ("newX: " + newX + " localPointerPosition: " + localPointerPosition.x + " positionOffset: " + positionOffset + " position: " + itemRectTransform.localPosition.x);
			if (newX > 0) {
				newX = 0;
			}

			itemRectTransform.localPosition = new Vector2 (newX, this.transform.localPosition.y);
		}
	}

	public void OnPointerClick(PointerEventData data) {
		if (Mathf.RoundToInt(itemRectTransform.localPosition.x) > (-buttonOffset - 5)) {
			lerpToReset = true;
			lerpToButtons = false;
		}
	}
}
