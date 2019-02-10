using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class TransactionData
{
    [Serializable]
    public class TransactionListContainer
    {
        public List<Transaction> transactionList = new List<Transaction>();
    }

    private string filePath = "Assets/Sticker Price/Data Files/Transactions.json";
    private FileUtility fileUtility = new FileUtility();
    [JsonProperty("transactionListContainer")]
    public TransactionListContainer transactionListContainer = new TransactionListContainer();

    public TransactionData()
    {
        ReadTransactions();
    }

    public TransactionData(Transaction newTransaction)
    {

    }

    private void WriteTransactionsToFile()
    { 
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(transactionListContainer, Formatting.Indented));
    }

    private void ReadTransactions()
    {
        try {
            transactionListContainer = JsonUtility.FromJson<TransactionListContainer>(fileUtility.readJson(filePath));
        }
        catch (JsonException jsonException)
        {
            Console.WriteLine(jsonException);
        }

        if (transactionListContainer == null)
        {
            transactionListContainer = new TransactionListContainer();
        }

        if (transactionListContainer.transactionList == null)
        {
            transactionListContainer.transactionList = new List<Transaction>();
        }
    }

    private void RemoveDuplicate(Transaction transaction)
    {
        ReadTransactions();
        //Does the transaction ID already exist in the list
        if (transactionListContainer.transactionList.Exists(x => x.GetTransactionID() == transaction.GetTransactionID()))
        {
            //Get the existing transacation and remove it from the list
            Transaction removeTrans = transactionListContainer.transactionList.Single(x => x.GetTransactionID() == transaction.GetTransactionID());
            transactionListContainer.transactionList.Remove(removeTrans);
        }
    }

    public void AddTransaction(Transaction transaction)
    {
        //Add the transaction to the list and serialize
        RemoveDuplicate(transaction);
        transactionListContainer.transactionList.Add(transaction);
        WriteTransactionsToFile();
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
