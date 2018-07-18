using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemRow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler
{

	//Public Variables

	public Text itemDescriptionText;
	public Text itemPriceText;

	//Private Variables
	private string itemDescription = "New Item";
	private float itemOriginalPrice = 10;
	private float itemPrice = 10;
	private ItemList parentList;
	private RectTransform itemRectTransform;
	private RectTransform contentRectTransform;
	private AdjustPanel adjustPanel;

	//Drag & Position Variables
	private Vector2 pointerStart;
	private Vector2 localRectStart;
	private Vector2 contentStart;
	//	private bool pointerDown = false;
	private int lerpMode = 0;
	private bool horizontalMode = false;
	private bool verticalMode = false;
	private float buttonOffset = -400;
	private float deleteOffset = -800;
	private float fullDeleteOffset = -1200;
	private float dragX;

	//Constants
	private const int NO_LERP = 0;
	private const int LERP_TO_BUTTONS = 1;
	private const int LERP_TO_RESET = 2;
	private const int LERP_TO_DELETE = 3;
	private const int LERP_TO_POSITION = 4;

	void Update ()
	{
		if (lerpMode == LERP_TO_BUTTONS) { //Lerping to show buttons
			float newX = Mathf.Lerp (itemRectTransform.localPosition.x, buttonOffset, Time.deltaTime * 5f);
			itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
			if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == buttonOffset) {
				lerpMode = NO_LERP;
			}
		} else if (lerpMode == LERP_TO_RESET) { //Lerping to hide all buttons
			float newX = Mathf.Lerp (itemRectTransform.localPosition.x, 0f, Time.deltaTime * 5f);
			itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
			if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == 0) {
				lerpMode = NO_LERP;
				//horizontalMode = false;//then let go of the mouse. In either case, we want to bring the row back to show buttons.
			}
		} else if (lerpMode == LERP_TO_DELETE) { //Lerping to show only Delete button
			float newX = Mathf.Lerp (itemRectTransform.localPosition.x, fullDeleteOffset, Time.deltaTime * 8);
			itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
			if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == fullDeleteOffset) {
				lerpMode = NO_LERP;
			}
		} else if (lerpMode == LERP_TO_POSITION) { //Lerping from delete only to mouse position
			float newX = Mathf.Lerp (itemRectTransform.localPosition.x, dragX, Time.deltaTime * 8);
			if (newX > 0) {
				newX = 0;
			}
			itemRectTransform.localPosition = new Vector2 (newX, itemRectTransform.localPosition.y);
			if (Mathf.RoundToInt (itemRectTransform.localPosition.x) == dragX) {
				lerpMode = NO_LERP;
			}
		}
	}

	void Awake ()
	{
		//initilizing variables
		itemRectTransform = (RectTransform)this.transform;
		contentRectTransform = this.transform.parent.GetComponent <RectTransform> ();
		lerpMode = NO_LERP;
		parentList = GetComponentInParent<ItemList> ();
		adjustPanel = parentList.transform.parent.parent.Find ("Popup").GetComponentInChildren <AdjustPanel> ();
	}

	public void OnPointerDown (PointerEventData data)
	{
		//when pointer is down, set position offset and reset all other rows in table
		lerpMode = NO_LERP;
		pointerStart = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		localRectStart = itemRectTransform.transform.localPosition;
		contentStart = contentRectTransform.transform.localPosition;
		parentList.resetOtherRows (this);
		parentList.stopScrolling ();
		verticalMode = false;
	}

	public void OnPointerUp (PointerEventData data)
	{
		verticalMode = false;
		horizontalMode = false;
		//when point is up find out where the row is, and where it needs to go
		int localX = Mathf.RoundToInt (itemRectTransform.localPosition.x);
		if ((localX < buttonOffset && localX >= deleteOffset) || (lerpMode == LERP_TO_POSITION && localX <= deleteOffset)) { //If row is between delete button trigger and show buttons trigger
			//or we were in the process of lerping to mouse position and are within delete trigger. 
			lerpMode = LERP_TO_BUTTONS;																				  //this would happen if the user just crossed the line from the delete only trigger and 
			//then let go of the mouse. In either case, we want to bring the row back to show buttons.
		} else if (localX >= buttonOffset) {//less than show buttons, go to reset								  
			lerpMode = LERP_TO_RESET;
		} else if (localX < deleteOffset && lerpMode != LERP_TO_POSITION) {//passed the delete threshold and we aren't lerping back to position, delete row. 
			DeleteButtonOnClickListener ();
		} 
	}

	public void OnDrag (PointerEventData data)
	{
		Vector2 pointerPosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		//Vector2 localPointerPosition;
		//RectTransformUtility.ScreenPointToLocalPointInRectangle (itemRectTransform, data.position, data.pressEventCamera, out localPointerPosition);

		//Debug.Log (" Horizontal:" + horizontalMode + " Vertical:" + verticalMode + " StartX:" + pointerStart.x + " PosX:" + pointerPosition.x + " NewX:" + (pointerPosition.x - pointerStart.x) + " LocalStart:" + localRectStart.x);
		//Debug.Log(itemRectTransform.anchoredPosition.y);
		if (!horizontalMode && !verticalMode) {
			if (Mathf.Abs (pointerPosition.x - pointerStart.x) > 15f) {
				horizontalMode = true;
			} else if (Mathf.Abs (pointerPosition.y - pointerStart.y) > 15f) {
				verticalMode = true;
			}
		} 

		if (horizontalMode && !parentList.getIsLerping ()) {//If the list is lerping to a position, we do not want to allow the user to scroll horizontally. 
			float newX = pointerPosition.x - pointerStart.x + localRectStart.x; //Example: width of row is 1000. Pointer position may be 800 which is right side of row. positionOffset stores value from -500 to 500, in this case it would be 300. So half
			dragX = newX;																																	//of the row width (500) + 800 position - 300 offset = 1000, which is the starting postion of the row. 
			if (newX > 0) { //trying to swipe right
				newX = 0;
			}
			if (newX < deleteOffset) { //passed delete threshold
				lerpMode = LERP_TO_DELETE;
			} else if (newX >= deleteOffset && Mathf.RoundToInt (itemRectTransform.localPosition.x) < deleteOffset) {//just passed threshold off of delete
				lerpMode = LERP_TO_POSITION;
			} else {//somewhere between start and delete threshold
				itemRectTransform.localPosition = new Vector2 (newX, this.transform.localPosition.y);
			}
		}

		if (verticalMode) {
			
			float newY = pointerPosition.y - pointerStart.y + contentStart.y; //Example: width of row is 1000. Pointer position may be 800 which is right side of row. positionOffset stores value from -500 to 500, in this case it would be 300. So half
			//Debug.Log ("pointerPosition:" + pointerPosition.y + " pointerStart:" + pointerStart.y + " parentListStart:" + contentStart.y + " newY:" + newY);
			dragX = newY;																																	//of the row width (500) + 800 position - 300 offset = 1000, which is the starting postion of the row. 
			if (newY > 0) { //trying to swipe right
				newY = 0;
			}


			contentRectTransform.transform.localPosition = new Vector2 (contentRectTransform.transform.localPosition.x, newY);
		}
	}

	public void OnPointerClick (PointerEventData data)
	{
		if (Mathf.RoundToInt (itemRectTransform.localPosition.x) > (-buttonOffset - 5)) {
			parentList.resetAllRows ();
		}
	}

	public void DeleteButtonOnClickListener ()
	{
		parentList.removeItem (this);
	}

	public void AdjustButtonOnClickListener ()
	{
		adjustPanel.openAdjustPanel (this);
		ResetRow ();
	}

	public void ResetRow ()
	{
		lerpMode = LERP_TO_RESET;
	}

	public void setItemDescription (string description)
	{
		itemDescription = description;
		itemDescriptionText.text = itemDescription;
	}

	public string getItemDescription ()
	{
		return itemDescription;
	}

	public void setItemPrice (float price)
	{
		itemPrice = price;
		itemPriceText.text = itemPrice.ToString ("C");
		parentList.calculateItemsAndPrice ();
	}

	public float getItemPrice ()
	{
		return itemPrice;
	}

	public void setItemOriginalPrice (float price)
	{
		itemOriginalPrice = price;
	}

	public float getItemOriginalPrice ()
	{
		return itemOriginalPrice;
	}
}
