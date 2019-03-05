using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class TransactionSummaryData 
{
    public string primaryKey;
    List<TransactionSummarySubDetailsData> transactionSummarySubDetailsList;

    public TransactionSummaryData(String primaryKey)
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

    public List<TransactionSummarySubDetailsData> getTransactionSummarySubDetailsData()
    {
        return transactionSummarySubDetailsList;
    }

    public void SetTransactionSummarySubDetailsData(List<TransactionSummarySubDetailsData> transactionSummarySubDetailsList)
    {
        this.transactionSummarySubDetailsList = transactionSummarySubDetailsList;
    }
}
