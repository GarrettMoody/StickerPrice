using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransactionSummaryGameObjectList
{
    private string primaryKey;
    private bool isExpanded = false;
    private List<GameObject> transactionSummaryGameObjectList;
    private GameObject parentGameObject;
    
    public TransactionSummaryGameObjectList(String primaryKey, GameObject parentGameObject)
    {
        this.primaryKey = primaryKey;
        transactionSummaryGameObjectList = new List<GameObject>();
        this.parentGameObject = parentGameObject;
    }

    public string GetPrimaryKey()
    {
        return primaryKey;
    }

    public void SetPrimaryKey(string primaryKey)
    {
        this.primaryKey = primaryKey;
    }

    public List<GameObject> GetTransactionSummaryGameObjects()
    {
        return transactionSummaryGameObjectList;
    }

    public void AddTransactionSummaryGameObject(GameObject newGameObject)
    {
        transactionSummaryGameObjectList.Add(newGameObject);
    }

    public bool IsExpanded()
    {
        return isExpanded;
    }

    public void SetExpandedFlag(bool isExpanded)
    {
        this.isExpanded = isExpanded;
    }
    public GameObject GetParentGameObject()
    {
        return parentGameObject;
    }

    public void SetParentGameObject(GameObject parentGameObject)
    {
        this.parentGameObject = parentGameObject;
    }
}
