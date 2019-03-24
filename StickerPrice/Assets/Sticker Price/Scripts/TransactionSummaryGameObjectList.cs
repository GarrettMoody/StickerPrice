using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransactionSummaryGameObjectList
{
    public string primaryKey;
    public bool visible = false;
    List<GameObject> transactionSummaryGameObjectList;

    public TransactionSummaryGameObjectList(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionSummaryGameObjectList = new List<GameObject>();
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

    public void SetTransactionSummaryGameObjects(List<GameObject> transactionSummaryGameObjectList)
    {
        this.transactionSummaryGameObjectList = transactionSummaryGameObjectList;
    }
}
