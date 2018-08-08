using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRPreview : MonoBehaviour {

    //Public Variables
    public float SNAPPING_THRESHOLD = 50f;
    public float LERP_SPEED = 5f;

    //Private Variables
    private ScrollRect scrollRect;
    private Vector2[] valueComponentCenters;
    private Vector2 scrollOffset;
    private GameObject content;
    private GameObject viewport;

    private bool isScrolling = false;
    private bool isMoving = false;
    private bool horizontal;

    // Use this for initialization
    void Start()
    {
        scrollRect = this.GetComponent<ScrollRect>();
        content = scrollRect.content.gameObject;
        viewport = scrollRect.viewport.gameObject;
        horizontal = scrollRect.horizontal;

        valueComponentCenters = new Vector2[content.transform.childCount];

        int i = 0;
        foreach(Transform child in content.transform) {
            RectTransform rect = child.gameObject.GetComponent<RectTransform>();
            valueComponentCenters[i] = rect.localPosition;
            i++;
        }

        scrollOffset = valueComponentCenters[0];

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        { //if its not moving there is nothing to update. 
            if (!isScrolling)
            { //We are moving, but too slow to scroll. Time to lerp to position.
                List<float> distancesFromCenter = new List<float>();

                if (horizontal)
                {
                    foreach (Vector2 position in valueComponentCenters)
                    {
                        distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.x + scrollOffset.x - position.x));
                    }
                } else {
                    foreach (Vector2 position in valueComponentCenters)
                    {
                        distancesFromCenter.Add(Mathf.Abs(-content.transform.localPosition.y + scrollOffset.y - position.y));
                    }
                }

                //get smallest distance from center
                float[] distanceArray = distancesFromCenter.ToArray();
                float minDistance = Mathf.Min(distanceArray);

                int minIndex = distancesFromCenter.IndexOf(minDistance);
                Vector2 newPosition = new Vector2(-valueComponentCenters[minIndex].x + scrollOffset.x, valueComponentCenters[minIndex].y - scrollOffset.y);
                LerpToFinish(newPosition);
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
        if(horizontal) {
            velocity = Mathf.Abs(scrollRect.velocity.x);
        } else {
            velocity = Mathf.Abs(scrollRect.velocity.y);    
        }

        //Is velocity high enough to keep scrolling?
        if (velocity > SNAPPING_THRESHOLD)
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