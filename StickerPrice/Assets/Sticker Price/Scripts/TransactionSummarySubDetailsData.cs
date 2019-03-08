using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummarySubDetailsData 
{
    public string primaryKey;
    List<TransactionSummaryDetailsData> transactionSummaryDetailsData;
    public float totalPrice = 0f;

    public TransactionSummarySubDetailsData(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionSummaryDetailsData = new List<TransactionSummaryDetailsData>();
    }

    public string GetPrimaryKey()
    {
        return primaryKey;
    }

    public void SetPrimaryKey(string primaryKey)
    {
        this.primaryKey = primaryKey;
    }

    public List<TransactionSummaryDetailsData> getTransactionSummaryDetailsData()
    {
        return transactionSummaryDetailsData;
    }

    public void SetTransactionSummaryDetailsData(List<TransactionSummaryDetailsData> transactionSummaryDetailsData)
    {
        this.transactionSummaryDetailsData = transactionSummaryDetailsData;
    }

    public float GetTotalPrice()
    {
        return totalPrice;
    }

    public void SetTotalPrice(float totalPrice)
    {
        this.totalPrice = totalPrice;
    }
}
