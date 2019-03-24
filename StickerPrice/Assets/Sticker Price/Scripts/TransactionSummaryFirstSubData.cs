using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummaryFirstSubData
{
    public string primaryKey;
    List<TransactionSummarySecondSubData> transactionSummarySecondSubDataList;
    public float totalPrice = 0f;

    public TransactionSummaryFirstSubData(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionSummarySecondSubDataList = new List<TransactionSummarySecondSubData>();
    }

    public string GetPrimaryKey()
    {
        return primaryKey;
    }

    public void SetPrimaryKey(string primaryKey)
    {
        this.primaryKey = primaryKey;
    }

    public float GetTotalPrice()
    {
        return totalPrice;
    }

    public void SetTotalPrice(float totalPrice)
    {
        this.totalPrice = totalPrice;
    }

    public List<TransactionSummarySecondSubData> GetTransactionSummarySecondSubDataList()
    {
        return transactionSummarySecondSubDataList;
    }

    public void SetTransactionSummarySecondSubDataList(List<TransactionSummarySecondSubData> transactionSummarySecondSubDataList)
    {
        this.transactionSummarySecondSubDataList = transactionSummarySecondSubDataList;
    }
}
