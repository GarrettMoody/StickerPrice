using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummaryData 
{
    public string primaryKey;
    List<TransactionSummaryFirstSubData> transactionSummaryFirstSubData;
    public float totalPrice = 0f;


    public TransactionSummaryData(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionSummaryFirstSubData = new List<TransactionSummaryFirstSubData>();
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

    public List<TransactionSummaryFirstSubData> GetTransactionSummaryFirstSubDataList()
    {
        return transactionSummaryFirstSubData;
    }

    public void SetTransactionSummaryFirstSubDataList(List<TransactionSummaryFirstSubData> transactionSummaryFirstSubData)
    {
        this.transactionSummaryFirstSubData = transactionSummaryFirstSubData;
    }
}
