using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ContentList : MonoBehaviour
{
    [Header("ContentList Variables")]
    public ScrollRect scrollRect;
    public RectTransform viewport;
    public RectTransform contentPanel;
    protected List<ContentRow> contentList;

    public virtual void Start()
    {
        contentList = new List<ContentRow>(contentPanel.GetComponentsInChildren<ContentRow>());
    }

    public virtual void Awake()
    {
        contentList = new List<ContentRow>(contentPanel.GetComponentsInChildren<ContentRow>());
    }

    public virtual void RemoveRow(ContentRow row)
    {
        contentList.Remove(row);
        Destroy(row.gameObject);
    }

    public virtual void RemoveAllRows()
    {
        while (contentList.Count > 0)
        {
            RemoveRow(contentList[0]);
        }
    }

    public void OnValueChange()
    {
        if (Mathf.Abs(scrollRect.velocity.y) < 10f)
        {
            scrollRect.StopMovement();
        }

        if (!Input.GetMouseButton(0))
        {
            ResetAllRows();
        }
    }

    public void ResetAllRows()
    {
        if (contentList != null)
        {
            foreach (ContentRow row in contentList)
            {
                row.ResetRow();
            }
        }
    }

    public void ResetOtherRows(ContentRow sourceRow)
    {
        if (contentList != null)
        {
            foreach (ContentRow row in contentList)
            {
                if (row != sourceRow)
                {
                    row.ResetRow();
                }
            }
        }
    }

    public virtual ContentRow AddRow(ContentRow row)
    {
        ContentRow newRow = Instantiate(row, contentPanel.transform);
        newRow.transform.SetAsLastSibling();
        contentList.Add(newRow);
        return newRow;
    }

    //public ContentRow AddRow()
    //{
    //    ContentRow newRow = Instantiate(contentRowPrefab, contentPanel.transform);
    //    newRow.transform.SetAsLastSibling();
    //    contentList.Add(newRow);
    //    return newRow;
    //}

}
