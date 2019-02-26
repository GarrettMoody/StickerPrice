using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ContentScroll : MonoBehaviour
{
    //It is assumed that each direct child component of the content panel contains a canvas component. This is used for sorting when
    //the components overlap each other in the scroll menu. 
    //The content panel can also not contain any Layout Group components or the scroll functionality will not work. 

    //Public Variables
    [Tooltip("The scroll speed at which snapping to the closest component begins.")]
    public float SNAPPING_VELOCITY_THRESHOLD = 50f; 
    [Tooltip("How fast components to snap to the center.")]
    public float LERP_SPEED = 5f;
    [Tooltip("Max distance from center in which the component will no longer scale. Set to 0 if scaling is undesired.")]
    public float SCALE_MAX_DISTANCE = 200f; 
    [Tooltip("The smallest scale (0-1) the component will scale to.")]
    public float SCALE_MIN_SCALE = .5f;
    [Tooltip("The color used on the Selection Indicator to signify which object is selected.")]
    public Color32 SELECTED_COLOR;
    [Tooltip("The color used on the Selection Indicator to show which object is not selected.")]
    public Color32 NON_SELECTED_COLOR;


    [HideInInspector]
    public ScrollRect scrollRect;
    [HideInInspector]
    public GameObject viewport;
    [HideInInspector]
    public GameObject content;
    [HideInInspector]
    public GameObject selectionIndicator;

    //Private Variables
    private GameObject[] contentComponents;
    private Vector2[] contentComponentCenters;
    private Vector2 scrollOffset;
    private List<float> distancesFromCenter = new List<float>();
    private bool isScrolling = false;
    private bool isMoving = false;
    private bool horizontal;
    private int selectionIndex = -1;

    //Events
    public delegate void OnValueChangeListener();
    public event OnValueChangeListener OnSelectionChange;

    private void Start()
    {
        InitializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        { //if its not moving there is nothing to update. 
            LoadDistancesFromCenter();
            UpdateSelectionIndicator();
            UpdateSelectionIndex();
            if (!isScrolling)
            { //We are moving, but too slow to scroll. Time to lerp to position.
                //get smallest distance from center
                int minIndex = GetMinDistantIndex();
                Vector2 newPosition = new Vector2(-contentComponentCenters[minIndex].x + scrollOffset.x, contentComponentCenters[minIndex].y - scrollOffset.y);
                LerpToFinish(newPosition);
            }

            ScaleComponents();
            SortComponentLayer();
        }
    }

    public void InitializeVariables()
    {
        horizontal = scrollRect.horizontal;
        contentComponents = new GameObject[content.transform.childCount];
        for (int i = 0; i < content.transform.childCount; i++)
        {
            contentComponents[i] = content.transform.GetChild(i).gameObject;
        }
        contentComponentCenters = new Vector2[contentComponents.Length];
        for (int i = 0; i < contentComponents.Length; i++)
        {
            RectTransform rect = contentComponents[i].gameObject.GetComponent<RectTransform>();
            contentComponentCenters[i] = contentComponents[i].gameObject.GetComponent<RectTransform>().anchoredPosition;
        }

        if (contentComponentCenters.Length > 0)
        {
            scrollOffset = contentComponentCenters[0];
        }
        LoadDistancesFromCenter();
        ScaleComponents();
        SortComponentLayer();
        UpdateSelectionIndicator();
        UpdateSelectionIndex();
    }

    private int GetMinDistantIndex() {
        //Gets the index of the component which contains the shortest distance from the center. 
        float[] distanceArray = distancesFromCenter.ToArray();
        float minDistance = Mathf.Min(distanceArray);

        int minIndex = distancesFromCenter.IndexOf(minDistance);
        return minIndex;
    }

    void UpdateSelectionIndicator() {
        //The selection indicator is not required, so updating it may not be neccessary
        if(selectionIndicator != null)
        {
            //Get the index of the content closest to the center and color the corresponding image in the selection indicator. 
            int minIndex = GetMinDistantIndex();
            int i = 0;
            foreach (Transform child in selectionIndicator.transform)
            {
                if (i == minIndex)
                {
                    child.gameObject.GetComponent<Image>().color = SELECTED_COLOR;
                }
                else
                {
                    child.gameObject.GetComponent<Image>().color = NON_SELECTED_COLOR;
                }
                i++;
            }
        }
    }

    void UpdateSelectionIndex()
    {
        //Updates the selection index. Calls the OnSelectionChange listener.
        int minIndex = GetMinDistantIndex();
        if (minIndex != selectionIndex)
        {
            selectionIndex = minIndex;
            if(OnSelectionChange != null)
            {
                OnSelectionChange();
            }
        }
    }

    void LoadDistancesFromCenter()
    {
        distancesFromCenter.Clear();
        //do math for horizontal movement
        if (horizontal)
        {
            foreach (Vector2 position in contentComponentCenters)
            {
                distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.x + scrollOffset.x - position.x));
            }
        }
        else
        { //math for verticle movement
            foreach (Vector2 position in contentComponentCenters)
            {
                distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.y + scrollOffset.y - position.y));
            }
        }

        return;
    }

    void ScaleComponents()
    {
        //For each component, change the scale accordingly
        for (int i = 0; i < contentComponents.Length; i++)
        {

            //if the component is within scalable threshold to be scaled
            if (distancesFromCenter[i] < SCALE_MAX_DISTANCE)
            {
                //Figure out the scale of the component by how far away the component is from the center
                float scale = (SCALE_MAX_DISTANCE - distancesFromCenter[i]) / SCALE_MAX_DISTANCE * SCALE_MIN_SCALE + SCALE_MIN_SCALE;
                //Set the scale of the component. 
                contentComponents[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
            }
            else //component too far away, set the component to the min scale
            {
                contentComponents[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(SCALE_MIN_SCALE, SCALE_MIN_SCALE, 1f);
            }
        }
    }

    void SortComponentLayer()
    {
        //Find the object closest to the center
        int minIndex = GetMinDistantIndex();

        //Component closest to center should be on top
        contentComponents[minIndex].gameObject.GetComponent<Canvas>().sortingOrder = contentComponents.Length;

        //This process starts from the center and moves outward. The component in the center gets the highest 
        //sorting order. The components on both sides get the sorting order - 1. Components outside of that 
        //get sorting order - 2 and so on until there are no more components on either side. 
        bool processing = true; //keep track of while loop
        int sortingOrder = contentComponents.Length - 1;
        int layer = 1; //keeps track of what layer we are on. 
        while (processing)
        {
            if (minIndex - layer >= 0)
            {
                contentComponents[minIndex - layer].gameObject.GetComponent<Canvas>().sortingOrder = sortingOrder;
            }
            if (minIndex + layer < contentComponents.Length)
            {
                contentComponents[minIndex + layer].gameObject.GetComponent<Canvas>().sortingOrder = sortingOrder;
            }
            if (minIndex - layer < 0 && minIndex + layer >= contentComponents.Length)
            {
                processing = false;
            }
            else
            {
                sortingOrder--;
                layer++;
            }
        }
    }

    void LerpToFinish(Vector2 pos)
    {
        float newY = Mathf.Lerp(content.transform.localPosition.y,
                                pos.y,
                                Time.deltaTime * LERP_SPEED);
        float newX = Mathf.Lerp(content.transform.localPosition.x,
                               pos.x,
                                Time.deltaTime * LERP_SPEED);

        Vector2 newPosition = new Vector2(newX, newY);
        content.transform.localPosition = newPosition;

    }

    public void OnValueChange()
    {
        float velocity = 0f;
        //Get velocity
        if (horizontal)
        {
            velocity = Mathf.Abs(scrollRect.velocity.x);
        }
        else
        {
            velocity = Mathf.Abs(scrollRect.velocity.y);
        }

        //Is velocity high enough to keep scrolling?
        if (velocity > SNAPPING_VELOCITY_THRESHOLD)
        {
            isScrolling = true;
        }
        else
        {
            isScrolling = false;
        }

        //Is velocity high enough to be considered moving?
        if (velocity < 2f)
        {
            isMoving = false;
            scrollRect.velocity = Vector2.zero;
        }
        else
        {
            isMoving = true;
        }
    }

    public GameObject GetSelectedComponent()
    {
        return contentComponents[selectionIndex];
    }

    public int GetSelectedIndex()
    {
        return selectionIndex;
    }

    public GameObject[] GetContentComponents()
    {
        return contentComponents;
    }

    public void RemoveContentComponents()
    {
        if(contentComponents != null)
        {
            foreach (GameObject contentGO in contentComponents)
            {
                Destroy(contentGO);
            }
            contentComponents = new GameObject[0];
        }

        if(content != null)
        {
            foreach (Transform contentGO in content.transform)
            {
                Destroy(contentGO.gameObject);
            }
            content.transform.DetachChildren();
        }

        contentComponents = null;
        contentComponentCenters = null;
        distancesFromCenter.Clear();
    }

    public void ScrollToContent(int index)
    {
        //Get distance from center
        float distance = distancesFromCenter[index];
        //If the new selection is right of the current position, subtract the position
        if(selectionIndex < index)
        {
            distance = -distance;
        }
        Vector2 position = new Vector2(content.transform.localPosition.x + distance, content.transform.localPosition.y);
        content.transform.localPosition = position;
        isMoving = true;
    }
}