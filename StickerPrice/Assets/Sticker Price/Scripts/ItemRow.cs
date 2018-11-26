using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemRow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler// IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

	//Public Variables

	public Text itemDescriptionText;
	public Text itemPriceText;
    public Button plusButton;
    public Button minusButton;
    public InputField quantityInputField;
    public ScrollRect horizontalScrollRect;
    public RectTransform horizontalContentPanel;

	//Private Variables
	private string itemDescription = "New Item";
	private float itemOriginalPrice = 10;
	private float itemPrice = 10;
    private int quantity = 1;
	private ItemList parentList;
	private RectTransform itemRectTransform;
	private RectTransform contentRectTransform;

	//Drag & Position Variables
	//private Vector2 pointerStart;
	//private Vector2 localRectStart;
	//private Vector2 contentStart;
    private Vector2 deletePointerThreshold;

	private int lerpMode = 0;
	//private bool horizontalMode = false;
	private bool verticalMode = false;
    private float BUTTON_OFFSET = .340f;
    private float DELETE_OFFSET = .700f;
    private float FULL_DELETE_OFFSET = 1;
	//private float dragX;

	//Constants
	private const int NO_LERP = 0;
	private const int LERP_TO_BUTTONS = 1;
	private const int LERP_TO_RESET = 2;
	private const int LERP_TO_DELETE = 3;
	private const int LERP_TO_POSITION = 4;

	void Update ()
	{
        Debug.Log(lerpMode);
		if (lerpMode == LERP_TO_BUTTONS) { //Lerping to show buttons
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, BUTTON_OFFSET, 5f);
		} else if (lerpMode == LERP_TO_RESET) { //Lerping to hide all buttons
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, 0f, 5f);
		} else if (lerpMode == LERP_TO_DELETE) { //Lerping to show only Delete button
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, FULL_DELETE_OFFSET, 8f);
  		} 
        else if (lerpMode == LERP_TO_POSITION) { //Lerping from delete only to mouse position
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, DELETE_OFFSET, 5f);
		}
	}

	void Awake ()
	{
		//initilizing variables
        itemRectTransform = this.GetComponent<RectTransform>();
		contentRectTransform = this.transform.parent.GetComponent <RectTransform> ();
		//lerpMode = NO_LERP;
		parentList = GetComponentInParent<ItemList> ();
	}

    void LerpToHorizontalPosition(float currentPosition, float targetPosition, float lerpTime) 
    {
        float newX = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * lerpTime);
        horizontalScrollRect.horizontalNormalizedPosition = newX;
        //Debug.Log(Mathf.Round(horizontalScrollRect.horizontalNormalizedPosition * 1000) + " " + (targetPosition * 1000));
        if (horizontalScrollRect.horizontalNormalizedPosition.ToString("F3") == targetPosition.ToString("F3"))
        {
            lerpMode = NO_LERP;
        }
    }
    //public void OnPointerDown (PointerEventData data)
    //{
    //	//when pointer is down, set position offset and reset all other rows in table
    //	lerpMode = NO_LERP;
    //	pointerStart = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
    //	localRectStart = itemRectTransform.transform.localPosition;
    //	contentStart = contentRectTransform.transform.localPosition;
    //	parentList.ResetOtherRows (this);
    //	parentList.StopScrolling ();
    //	verticalMode = false;
    //}

    //public void OnPointerUp (PointerEventData data)
    //{
    //	verticalMode = false;
    //	horizontalMode = false;
    //	//when point is up find out where the row is, and where it needs to go
    //       int localX = Mathf.RoundToInt (itemRectTransform.anchoredPosition.x);
    //	if ((localX < buttonOffset && localX >= deleteOffset) || (lerpMode == LERP_TO_POSITION && localX <= deleteOffset)) { //If row is between delete button trigger and show buttons trigger
    //		//or we were in the process of lerping to mouse position and are within delete trigger. 
    //		lerpMode = LERP_TO_BUTTONS;				//this would happen if the user just crossed the line from the delete only trigger and 
    //		//then let go of the mouse. In either case, we want to bring the row back to show buttons.
    //	} else if (localX >= buttonOffset) {//less than show buttons, go to reset								  
    //		lerpMode = LERP_TO_RESET;
    //	} else if (localX < deleteOffset && lerpMode != LERP_TO_POSITION) {//passed the delete threshold and we aren't lerping back to position, delete row. 
    //		DeleteButtonOnClickListener ();
    //	} 
    //}

    //public void OnDrag (PointerEventData data)
    //{
    //	Vector2 pointerPosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

    //	if (!horizontalMode && !verticalMode) {
    //		if (Mathf.Abs (pointerPosition.x - pointerStart.x) > 15f) {
    //			horizontalMode = true;
    //               Debug.Log("horizontal");
    //		} else if (Mathf.Abs (pointerPosition.y - pointerStart.y) > 15f) {
    //			verticalMode = true;
    //               Debug.Log("Vertical");
    //		}
    //	} 

    //	if (horizontalMode && !parentList.GetIsLerping ()) {//If the list is lerping to a position, we do not want to allow the user to scroll horizontally. 
    //		float newX = pointerPosition.x - pointerStart.x + localRectStart.x; //Example: width of row is 1000. Pointer position may be 800 which is right side of row. positionOffset stores value from -500 to 500, in this case it would be 300. So half
    //		dragX = newX;																																	//of the row width (500) + 800 position - 300 offset = 1000, which is the starting postion of the row. 
    //		if (newX > 0) { //trying to swipe right
    //			newX = 0;
    //		}
    //		if (newX < deleteOffset) { //passed delete threshold
    //			lerpMode = LERP_TO_DELETE;
    //		} else if (newX >= deleteOffset && Mathf.RoundToInt (itemRectTransform.localPosition.x) < deleteOffset) {//just passed threshold off of delete
    //			lerpMode = LERP_TO_POSITION;
    //		} else {//somewhere between start and delete threshold
    //			itemRectTransform.localPosition = new Vector2 (newX, this.transform.localPosition.y);
    //		}
    //	}

    //	if (verticalMode) {

    //		float newY = pointerPosition.y - pointerStart.y + contentStart.y; //Example: width of row is 1000. Pointer position may be 800 which is right side of row. positionOffset stores value from -500 to 500, in this case it would be 300. So half
    //		//Debug.Log ("pointerPosition:" + pointerPosition.y + " pointerStart:" + pointerStart.y + " parentListStart:" + contentStart.y + " newY:" + newY);
    //		dragX = newY;																																	//of the row width (500) + 800 position - 300 offset = 1000, which is the starting postion of the row. 
    //		if (newY > 0) { //trying to swipe right
    //			newY = 0;
    //		}


    //		contentRectTransform.transform.localPosition = new Vector2 (contentRectTransform.transform.localPosition.x, newY);
    //	}
    //}

    //public void OnPointerClick (PointerEventData data)
    //{
    //if (Mathf.RoundToInt (itemRectTransform.localPosition.x) > (-buttonOffset - 5)) {
    //	parentList.ResetAllRows ();
    //}
    //}

    public void OnPlusButtonClick() {
        SetQuantity(quantity + 1);
    }

    public void OnMinusButtonClick() {
        SetQuantity(quantity - 1);
    }

	public void DeleteButtonOnClickListener ()
	{
		parentList.RemoveItem (this);
	}

	//public void AdjustButtonOnClickListener ()
	//{
	//	ResetRow ();
	//}

	public void ResetRow ()
	{
		lerpMode = LERP_TO_RESET;
	}

	public void SetItemDescription (string description)
	{
		itemDescription = description;
		itemDescriptionText.text = itemDescription;
	}

	public string GetItemDescription ()
	{
		return itemDescription;
	}

	public void SetItemPrice (float price)
	{
		itemPrice = price;
		itemPriceText.text = itemPrice.ToString ("C");
		parentList.CalculateItemsAndPrice ();
	}

	public float GetItemPrice ()
	{
		return itemPrice;
	}

	public void SetItemOriginalPrice (float price)
	{
		itemOriginalPrice = price;
	}

	public float GetItemOriginalPrice ()
	{
		return itemOriginalPrice;
	}

    public int GetQuantity() {
        return quantity;
    }

    public void SetQuantity(int value) {
        if (value >= 0 && value <= 99)
        {
            quantity = value;
            UpdateQuantity();
        }
    }

    public void UpdateQuantity() {
        quantityInputField.text = quantity.ToString();
        parentList.CalculateItemsAndPrice();
    }

    public void OnQuantityInputFieldValueChanged() {
        quantity = int.Parse(quantityInputField.text);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Get drag distance
        float horizontalDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
        float verticalDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
        lerpMode = NO_LERP;

        //If dragging horizontal is greater than vertical distance
        if (horizontalDistance > verticalDistance) {
            parentList.scrollRect.enabled = false;
            horizontalScrollRect.enabled = true;
            verticalMode = false;
        } else {
            parentList.scrollRect.enabled = true;
            horizontalScrollRect.enabled = false;
            verticalMode = true;
            parentList.scrollRect.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (verticalMode)
        { //Scrolling Vertically
            parentList.scrollRect.OnDrag(eventData);
        }
        else
        {
            if (lerpMode == LERP_TO_DELETE) //If lerping to delete dont do anything else
            {
                eventData.dragging = false;
            } else {
                //Scrolling horizontally
                if (horizontalScrollRect.horizontalNormalizedPosition >= DELETE_OFFSET) //Scroll is past delete threshold
                {
                    if (lerpMode != LERP_TO_DELETE)
                    { //If first time crossing delete threshold
                        lerpMode = LERP_TO_DELETE;
                        deletePointerThreshold = eventData.position; //Set threshold for mouse pointer
                    }
                    if (eventData.position.x >= deletePointerThreshold.x)
                    {
                        Debug.Log("UNDELETE");
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (verticalMode) {
            parentList.scrollRect.OnEndDrag(eventData);
        } else {
            if (horizontalScrollRect.horizontalNormalizedPosition > DELETE_OFFSET)
            {
                Debug.Log("DELETE");
                lerpMode = LERP_TO_DELETE;
            }
            else if (horizontalScrollRect.horizontalNormalizedPosition < DELETE_OFFSET && horizontalScrollRect.horizontalNormalizedPosition >= BUTTON_OFFSET)
            {
                Debug.Log("BUTTON");
                lerpMode = LERP_TO_BUTTONS;
            }
            else if (horizontalScrollRect.horizontalNormalizedPosition < BUTTON_OFFSET)
            {
                Debug.Log("RESET");
                lerpMode = LERP_TO_RESET;
            }
        }
    }
}
