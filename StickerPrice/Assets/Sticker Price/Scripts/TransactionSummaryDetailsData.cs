using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummaryDetailsData 
{
    public string primaryKey;
    List<Transaction> transactionList;
    public float totalPrice = 0f;

    public TransactionSummaryDetailsData(String primaryKey)
    {
        this.primaryKey = primaryKey;
        transactionList = new List<Transaction>();
    }

    public string GetPrimaryKey()
    {
        return primaryKey;
    }

    public void SetPrimaryKey(string primaryKey)
    {
        this.primaryKey = primaryKey;
    }

    public List<Transaction> getTransactionList()
    {
        return transactionList;
    }

    public void SetTransactionList(List<Transaction> transactionList)
    {
        this.transactionList = transactionList;
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
