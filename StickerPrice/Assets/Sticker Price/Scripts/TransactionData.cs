using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class TransactionData
{
    public class TransactionListContainer
    {
        public List<Transaction> transactionList = new List<Transaction>();
    }

    private string filePath = "Assets/Sticker Price/Data Files/Transactions.json";
    private FileUtility fileUtility = new FileUtility();
    public TransactionListContainer transactionListContainer = new TransactionListContainer();

    public TransactionData()
    {
        ReadTransactions();
    }

    public void WriteTransaction(Transaction transaction)
    { 
        ReadTransactions();
        //Does the transaction ID already exist in the list
        if(transactionListContainer.transactionList.Exists(x => x.GetTransactionID() == transaction.GetTransactionID()))
        {
            //Get the existing transacation and remove it from the list
            Transaction removeTrans = transactionListContainer.transactionList.Single(x => x.GetTransactionID() == transaction.GetTransactionID());
            transactionListContainer.transactionList.Remove(removeTrans);
        }

        //Add the transaction to the list and serialize
        transactionListContainer.transactionList.Add(transaction);
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath,  JsonConvert.SerializeObject(transactionListContainer));
    }

    public void ReadTransactions()
    {
        transactionListContainer.transactionList = JsonConvert.DeserializeObject<List<Transaction>>(fileUtility.readJson(filePath));
        if (transactionListContainer.transactionList == null)
        {
            transactionListContainer.transactionList = new List<Transaction>();
        }
    }

    public void RemoveDuplicate()
    {
        ReadTransactions();
        transactionListContainer.transactionList = transactionListContainer.transactionList.Distinct().ToList();

    }

    public List<Transaction> GetAllTransactions()
    {
        ReadTransactions();
        return transactionListContainer.transactionList;
    }

    public float NextTransactionID()
    {
        ReadTransactions();
        float currentTransactionID = 0;
        if (transactionListContainer.transactionList != null)
        {
            foreach(Transaction td in transactionListContainer.transactionList)
        {
                if (td.GetTransactionID() > currentTransactionID)
                {
                    currentTransactionID = td.GetTransactionID();
                }
            }
        }
        float nextTransactionID = currentTransactionID + 1;
        return nextTransactionID;
    }
}
