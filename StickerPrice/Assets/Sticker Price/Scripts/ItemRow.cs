using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemRow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler {

	//Public Variables

	public Text itemDescriptionText;
	public Text itemPriceText;

	//Private Variables
	private string itemDescription;
	private float itemPrice;

	//Drag & Position Variables
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
			if (lerpToButtons) { //Lerping to show buttons
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, -buttonOffset, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == -buttonOffset) {
					lerpToButtons = false;
				}
			} else if (lerpToReset) { //Lerping to hide all buttons
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, 0f, Time.deltaTime * 5f);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == 0) {
					lerpToReset = false;
				}
			} else if (lerpToDelete) { //Lerping to show only Delete button
				float newX = Mathf.Lerp (itemRectTransform.localPosition.x, -fullDeleteOffset, Time.deltaTime * 8);
				itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
				if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == -fullDeleteOffset) {
					lerpToDelete = false;
				}
			} else if (lerpToPosition) { //Lerping from delete only to mouse position
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
		//initilizing variables
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
		//when pointer is down, set position offset and reset all other rows in table
		lerpToReset = false;
		lerpToButtons = false;
		lerpToDelete = false;
		lerpToPosition = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (itemRectTransform, data.position, data.pressEventCamera, out positionOffset);
		parentList.resetOtherRows (this);
	}

	public void OnPointerUp(PointerEventData data) {
		//when point is up find out where the row is, and where it needs to go
		int localX = Mathf.RoundToInt (itemRectTransform.localPosition.x);
		if ((localX < -buttonOffset && localX >= -deleteOffset) || (lerpToPosition && localX <= -deleteOffset)) { //If row is between delete button trigger and show buttons trigger
			lerpToPosition = false;																				  //or we were in the process of lerping to mouse position and are within delete trigger. 
			lerpToButtons = true;																				  //this would happen if the user just crossed the line from the delete only trigger and 
		} else if (localX >= -buttonOffset) {//less than show buttons, go to reset								  //then let go of the mouse. In either case, we want to bring the row back to show buttons.
			lerpToButtons = false;
			lerpToPosition = false;
			lerpToReset = true;
		} else if (localX < -deleteOffset && !lerpToPosition) {//passed the delete threshold and we aren't lerping back to position, delete row. 
			DeleteButtonOnClickListener ();
		} 
	}

	public void OnDrag(PointerEventData data) {
		Vector2 localPointerPosition;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRectTransform, data.position, data.pressEventCamera, out localPointerPosition)) {//getting local pointer position
			float newX = localPointerPosition.x + (itemRectTransform.rect.width / 2) - positionOffset.x; //Example: width of row is 1000. Pointer position may be 800 which is right side of row. positionOffset stores value from -500 to 500, in this case it would be 300. So half
			dragX = newX;																																	//of the row width (500) + 800 position - 300 offset = 1000, which is the starting postion of the row. 
			//Debug.Log ("newX: " + newX + " localX: " + itemRectTransform.localPosition.x + " Delete: " + lerpToDelete + " Position: " + lerpToPosition);
			if (newX > 0) { //trying to swipe right
				newX = 0;
			}
			if (newX < -deleteOffset) { //passed delete threshold
				lerpToDelete = true;
				lerpToPosition = false;
			} else if (newX >= -deleteOffset && Mathf.RoundToInt(itemRectTransform.localPosition.x) < -deleteOffset) {//just passed threshold off of delete
				lerpToDelete = false; 
				lerpToPosition = true;
			} else {//somewhere between start and delete threshold
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

	public void setItemDescription(string description) {
		itemDescription = description;
		itemDescriptionText.text = itemDescription;
	}

	public string getItemDescription() {
		return itemDescription;
	}

	public void setItemPrice(float price) {
		itemPrice = price;
		itemPriceText.text = itemPrice.ToString();
	}

	public float getItemPrice() {
		return itemPrice;
	}
}
