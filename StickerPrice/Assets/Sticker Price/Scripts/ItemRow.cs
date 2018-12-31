using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemRow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerUpHandler // IPointerDownHandler
{

	//Public Variables

	public Text itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Button plusButton;
    public Button minusButton;
    public InputField quantityInputField;
    public ScrollRect horizontalScrollRect;
    public RectTransform horizontalContentPanel;
    public RawImage QRCode;

	//Private Variables
	private string itemDescription = "New Item";
	private float itemOriginalPrice = 10;
	private float itemPrice = 10;
    private int quantity = 1;
	private ItemList parentList;
	private RectTransform itemRectTransform;
	private RectTransform contentRectTransform;
    private Vector2 deletePointerThreshold;
    private string scanString;

	private int lerpMode = 0;
	private bool verticalMode = false;
    private float BUTTON_OFFSET = .340f;
    private float DELETE_OFFSET = .650f;
    private float FULL_DELETE_OFFSET = 1;

	//Constants
	private const int NO_LERP = 0;
	private const int LERP_TO_BUTTONS = 1;
	private const int LERP_TO_RESET = 2;
	private const int LERP_TO_DELETE = 3;
	private const int LERP_TO_POSITION = 4;

	void Update ()
	{
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
		parentList = GetComponentInParent<ItemList> ();
	}

    void LerpToHorizontalPosition(float currentPosition, float targetPosition, float lerpTime) 
    {
        float newX = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * lerpTime);
        horizontalScrollRect.horizontalNormalizedPosition = newX;
        if (horizontalScrollRect.horizontalNormalizedPosition.ToString("F3") == targetPosition.ToString("F3"))
        {
            lerpMode = NO_LERP;
        }
    }

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

	public void AdjustButtonOnClickListener ()
	{
		ResetRow ();
        parentList.OpenPriceAdjustPanel(this);
	}

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
        if (lerpMode == LERP_TO_DELETE && eventData.position.x <= deletePointerThreshold.x) //If lerping_to_delete and pointer is still passed lerping threshold, let it lerp, don't begin drag
        {
            eventData.dragging = false;
        }
        else
        {
            //Get drag distance
            float horizontalDistance = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
            float verticalDistance = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
            lerpMode = NO_LERP;


            //If dragging horizontal is greater than vertical distance
            if (horizontalDistance > verticalDistance)
            {
                parentList.scrollRect.enabled = false;
                horizontalScrollRect.enabled = true;
                verticalMode = false;
            }
            else
            {
                parentList.scrollRect.enabled = true;
                horizontalScrollRect.enabled = false;
                verticalMode = true;
                parentList.scrollRect.OnBeginDrag(eventData);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (verticalMode)
        { //Scrolling Vertically
            parentList.scrollRect.OnDrag(eventData);
        }
        else //scrolling horizontally
        {

            if (lerpMode == NO_LERP) { 
                if (horizontalScrollRect.horizontalNormalizedPosition >= DELETE_OFFSET) { //Scroll is passed delete threshold
                    if(horizontalScrollRect.horizontalNormalizedPosition.ToString("F3") == FULL_DELETE_OFFSET.ToString("F3")) { //If fully lerped to delete
                        if(eventData.position.x > deletePointerThreshold.x) { //Crossed over the delete threshold again for first time
                            lerpMode = LERP_TO_POSITION;
                            deletePointerThreshold = Vector2.zero;
                        } else { //dragging within delete threshold but already fully lerped to delete
                            eventData.dragging = false;
                        }
                    } else {//Scroll just passed delete threshold for first time
                        lerpMode = LERP_TO_DELETE;
                        deletePointerThreshold = eventData.position;
                    }
                }
            } else if (lerpMode == LERP_TO_DELETE) {
                if(eventData.position.x > deletePointerThreshold.x) { //crossed over delete threshold while lerping to delete
                    lerpMode = LERP_TO_POSITION;
                }
                eventData.dragging = false;
            } else if (lerpMode == LERP_TO_POSITION) {

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
                parentList.RemoveItem(this);
            }
            else if (horizontalScrollRect.horizontalNormalizedPosition < DELETE_OFFSET && horizontalScrollRect.horizontalNormalizedPosition >= BUTTON_OFFSET)
            {
                lerpMode = LERP_TO_BUTTONS;
            }
            else if (horizontalScrollRect.horizontalNormalizedPosition < BUTTON_OFFSET)
            {
                lerpMode = LERP_TO_RESET;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetRow();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (horizontalScrollRect.horizontalNormalizedPosition > DELETE_OFFSET) {
            DeleteButtonOnClickListener();
        }
    }

    public string GetScanString () {
        return scanString;
    }

    public void SetScanString(string value) {
        scanString = value;
        Texture2D qrCode = QRCodeGenerator.CreateQRCode(scanString);
        QRCode.texture = qrCode;
    }
}
