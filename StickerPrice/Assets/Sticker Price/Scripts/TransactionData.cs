using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class TransactionData
{
    private string filePath = "Assets/Sticker Price/Data Files/Transactions.json";
    private FileUtility fileUtility = new FileUtility();
    [SerializeField, HideInInspector] private List<Transaction> transactionList = new List<Transaction>();

    private class TransactionListContainer {
        public List<Transaction> transactionList = new List<Transaction>();
    }

    public TransactionData()
    {
        ReadTransactions();
    }

    public void WriteTransaction(Transaction transaction)
    {
        ReadTransactions();
        //Does the transaction ID already exist in the list
        if(transactionList.Exists(x => x.GetTransactionID() == transaction.GetTransactionID()))
        {
            //Get the existing transacation and remove it from the list
            Transaction removeTrans = transactionList.Single(x => x.GetTransactionID() == transaction.GetTransactionID());
            transactionList.Remove(removeTrans);
        }

        //Add the transaction to the list and serialize
        transactionList.Add(transaction);
        fileUtility.clearFile(filePath);
        TransactionListContainer transactionListContainer = new TransactionListContainer();
        transactionListContainer.transactionList = transactionList;
        fileUtility.writeJson(filePath,  JsonConvert.SerializeObject(transactionListContainer));
    }

    public void ReadTransactions()
    {
        transactionList = JsonConvert.DeserializeObject<List<Transaction>>(fileUtility.readJson(filePath));
        if (transactionList == null)
        {
            transactionList = new List<Transaction>();
        }
    }

    public void RemoveDuplicate()
    {
        ReadTransactions();
        transactionList = transactionList.Distinct().ToList();

    }

    public List<Transaction> GetAllTransactions()
    {
        ReadTransactions();
        return transactionList;
    }

    public float NextTransactionID()
    {
        ReadTransactions();
        float currentTransactionID = 0;
        if (transactionList != null)
        {
            foreach(Transaction td in transactionList)
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
