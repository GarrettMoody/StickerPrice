using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ContentScroll : MonoBehaviour
{

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
    private GameObject[] valueComponent;
    private Vector2[] valueComponentCenters;
    private Vector2 scrollOffset;
    private List<float> distancesFromCenter = new List<float>();
    private bool isScrolling = false;
    private bool isMoving = false;
    private bool horizontal;

    // Use this for initialization
    void Start()
    {
        horizontal = scrollRect.horizontal;
        valueComponent = new GameObject[content.transform.childCount];
        for (int i = 0; i < content.transform.childCount; i++)
        {
            valueComponent[i] = content.transform.GetChild(i).gameObject;
        }
        valueComponentCenters = new Vector2[valueComponent.Length];
        for (int i = 0; i < valueComponent.Length; i++)
        {
            valueComponentCenters[i] = valueComponent[i].gameObject.GetComponent<RectTransform>().localPosition;
        }

        scrollOffset = valueComponentCenters[0];
        LoadDistancesFromCenter();
        ScaleComponents();
        SortComponentLayer();
        UpdateSelectionIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        { //if its not moving there is nothing to update. 
            LoadDistancesFromCenter();
            UpdateSelectionIndicator();
            if (!isScrolling)
            { //We are moving, but too slow to scroll. Time to lerp to position.
                //get smallest distance from center
                int minIndex = GetMinDistantIndex();
                Vector2 newPosition = new Vector2(-valueComponentCenters[minIndex].x + scrollOffset.x, valueComponentCenters[minIndex].y - scrollOffset.y);
                LerpToFinish(newPosition);
            }

            ScaleComponents();
            SortComponentLayer();
        }
    }

    private int GetMinDistantIndex() {
        //Gets the index of the component which contains the shortest distance from the center. 
        float[] distanceArray = distancesFromCenter.ToArray();
        float minDistance = Mathf.Min(distanceArray);

        int minIndex = distancesFromCenter.IndexOf(minDistance);
        return minIndex;
    }

    void UpdateSelectionIndicator() {
        //Get the index of the content closest to the center and color the corresponding image in the selection indicator. 
        int minIndex = GetMinDistantIndex();
        int i = 0;
        foreach(Transform child in selectionIndicator.transform) {
            if (i == minIndex) {
                child.gameObject.GetComponent<Image>().color = SELECTED_COLOR;
            } else {
                child.gameObject.GetComponent<Image>().color = NON_SELECTED_COLOR;
            }
            i++;
        }
    }

    void LoadDistancesFromCenter()
    {
        distancesFromCenter.Clear();
        //do math for horizontal movement
        if (horizontal)
        {
            foreach (Vector2 position in valueComponentCenters)
            {
                distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.x + scrollOffset.x - position.x));
            }
        }
        else
        { //math for verticle movement
            foreach (Vector2 position in valueComponentCenters)
            {
                distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.y + scrollOffset.y - position.y));
            }
        }

        return;
    }

    void ScaleComponents()
    {
        //For each component, change the scale accordingly
        for (int i = 0; i < valueComponent.Length; i++)
        {

            //if the component is within scalable threshold to be scaled
            if (distancesFromCenter[i] < SCALE_MAX_DISTANCE)
            {
                //Figure out the scale of the component by how far away the component is from the center
                float scale = (SCALE_MAX_DISTANCE - distancesFromCenter[i]) / SCALE_MAX_DISTANCE * SCALE_MIN_SCALE + SCALE_MIN_SCALE;
                //Set the scale of the component. 
                valueComponent[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
            }
            else //component too far away, set the component to the min scale
            {
                valueComponent[i].gameObject.GetComponent<RectTransform>().localScale = new Vector3(SCALE_MIN_SCALE, SCALE_MIN_SCALE, 1f);
            }
        }
    }

    void SortComponentLayer()
    {
        //Find the object closest to the center
        int minIndex = GetMinDistantIndex();

        //Component closest to center should be on top
        valueComponent[minIndex].gameObject.GetComponent<Canvas>().sortingOrder = valueComponent.Length;

        //This process starts from the center and moves outward. The component in the center gets the highest 
        //sorting order. The components on both sides get the sorting order - 1. Components outside of that 
        //get sorting order - 2 and so on until there are no more components on either side. 
        bool processing = true; //keep track of while loop
        int sortingOrder = valueComponent.Length - 1;
        int layer = 1; //keeps track of what layer we are on. 
        while (processing)
        {
            if (minIndex - layer >= 0)
            {
                valueComponent[minIndex - layer].gameObject.GetComponent<Canvas>().sortingOrder = sortingOrder;
            }
            if (minIndex + layer < valueComponent.Length)
            {
                valueComponent[minIndex + layer].gameObject.GetComponent<Canvas>().sortingOrder = sortingOrder;
            }
            if (minIndex - layer < 0 && minIndex + layer >= valueComponent.Length)
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
}