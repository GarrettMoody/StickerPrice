using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummarySecondSubData
{
    public string primaryKey;
    List<TransactionSummarySubDetailsData> transactionSummarySubDetailsList;
    public float totalPrice = 0f;

    public TransactionSummarySecondSubData(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionSummarySubDetailsList = new List<TransactionSummarySubDetailsData>();
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

    public List<TransactionSummarySubDetailsData> GetTransactionSummarySubDetailsData()
    {
        return transactionSummarySubDetailsList;
    }

    public void SetTransactionSummarySubDetailsData(List<TransactionSummarySubDetailsData> transactionSummarySubDetailsList)
    {
        this.transactionSummarySubDetailsList = transactionSummarySubDetailsList;
    }
}
