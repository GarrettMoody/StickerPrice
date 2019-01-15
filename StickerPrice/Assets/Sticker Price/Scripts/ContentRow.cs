using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ContentRow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerUpHandler // IPointerDownHandler
{
    [Header("ContentRow Properties")]
    [Tooltip("The horizontal scroll rect of the row.")]
    public ScrollRect horizontalScrollRect; 

    [Tooltip("The % offset when the row is resting on buttons.")]
    public float BUTTON_OFFSET = .340f;
    [Tooltip("The % offset to lerp to full button.")]
    public float DEFAULT_OFFSET = .650f;

    public ContentList parentList;
    private RectTransform contentRectTransform;
    private RectTransform rowRectTransform;

    private int lerpMode = 0; //The lerp status. Is the row currently lerping to a position? 
    private bool verticalMode = false; //The flag of whether the user is scrolling vertically in the list or horizontally on the row
    private readonly float FULL_DEFAULT_OFFSET = 1; //The % offset when the default button is active

    //Constants
    private const int NO_LERP = 0; //Scroll is not lerping
    private const int LERP_TO_BUTTONS = 1; //Sroll is lerping to rest on buttons (show buttons)
    private const int LERP_TO_RESET = 2; //Scroll is lerping to default row (hides buttons)
    private const int LERP_TO_DEFAULT_BUTTON = 3; //Scroll is lerping to default button
    private const int LERP_TO_POSITION = 4; //Scroll is lerping to position of the pointer

    private Vector2 defaultButtonThreshold;

    void Update()
    {
        if (lerpMode == LERP_TO_BUTTONS)
        { //Lerping to show buttons
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, BUTTON_OFFSET, 5f);
        }
        else if (lerpMode == LERP_TO_RESET)
        { //Lerping to hide all buttons
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, 0f, 5f);
        }
        else if (lerpMode == LERP_TO_DEFAULT_BUTTON)
        { //Lerping to show only Delete button
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, FULL_DEFAULT_OFFSET, 8f);
        }
        else if (lerpMode == LERP_TO_POSITION)
        { //Lerping from delete only to mouse position
            LerpToHorizontalPosition(horizontalScrollRect.horizontalNormalizedPosition, DEFAULT_OFFSET, 5f);
        }
    }

    virtual public void Awake()
    {
        //initilizing variables
        rowRectTransform = this.GetComponent<RectTransform>();
        contentRectTransform = this.transform.parent.GetComponent<RectTransform>();
        parentList = GetComponentInParent<ContentList>();
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

    public void ResetRow()
    {
        lerpMode = LERP_TO_RESET;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (lerpMode == LERP_TO_DEFAULT_BUTTON && eventData.position.x <= defaultButtonThreshold.x) //If lerping_to_delete and pointer is still passed lerping threshold, let it lerp, don't begin drag
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

            if (lerpMode == NO_LERP)
            {
                if (horizontalScrollRect.horizontalNormalizedPosition >= DEFAULT_OFFSET)
                { //Scroll is passed delete threshold
                    if (horizontalScrollRect.horizontalNormalizedPosition.ToString("F3") == FULL_DEFAULT_OFFSET.ToString("F3"))
                    { //If fully lerped to delete
                        if (eventData.position.x > defaultButtonThreshold.x)
                        { //Crossed over the delete threshold again for first time
                            lerpMode = LERP_TO_POSITION;
                            defaultButtonThreshold = Vector2.zero;
                        }
                        else
                        { //dragging within delete threshold but already fully lerped to delete
                            eventData.dragging = false;
                        }
                    }
                    else
                    {//Scroll just passed delete threshold for first time
                        lerpMode = LERP_TO_DEFAULT_BUTTON;
                        defaultButtonThreshold = eventData.position;
                    }
                }
            }
            else if (lerpMode == LERP_TO_DEFAULT_BUTTON)
            {
                if (eventData.position.x > defaultButtonThreshold.x)
                { //crossed over delete threshold while lerping to delete
                    lerpMode = LERP_TO_POSITION;
                }
                eventData.dragging = false;
            }
            else if (lerpMode == LERP_TO_POSITION)
            {

            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (verticalMode)
        {
            parentList.scrollRect.OnEndDrag(eventData);
        }
        else
        {
            if (horizontalScrollRect.horizontalNormalizedPosition > DEFAULT_OFFSET)
            {
                parentList.RemoveRow(this);
            }
            else if (horizontalScrollRect.horizontalNormalizedPosition < DEFAULT_OFFSET && horizontalScrollRect.horizontalNormalizedPosition >= BUTTON_OFFSET)
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
        if (horizontalScrollRect.horizontalNormalizedPosition > DEFAULT_OFFSET)
        {
            DefaultButtonOnClickListener();
        }
    }

    public abstract void DefaultButtonOnClickListener();
}
