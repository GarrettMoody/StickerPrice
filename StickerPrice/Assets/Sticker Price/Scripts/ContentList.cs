using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class ContentList : MonoBehaviour
{
    [Header("ContentList Variables")]
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] protected RectTransform contentPanel;
    [SerializeField, HideInInspector] protected List<ContentRow> contentList;

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

    public virtual void AddRow(ContentRow row)
    {
        row.transform.SetParent(contentPanel.transform);
        row.transform.SetAsLastSibling();
        contentList.Add(row);
    }

    public virtual List<ContentRow> GetRows()
    {
        return contentList;
    }

    public ScrollRect GetScrollRect()
    {
        return scrollRect;
    }

    public RectTransform GetContentPanel()
    {
        return contentPanel;
    }
}
