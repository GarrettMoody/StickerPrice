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
    private Transaction newTransaction = new Transaction();
    [JsonProperty("transactionListContainer")]
    public TransactionListContainer transactionListContainer = new TransactionListContainer();

    public TransactionData()
    {
        ReadTransactions();
    }

    public TransactionData(Transaction newTransaction)
    {
        this.newTransaction = newTransaction;
    }

    public void WriteTransaction()
    { 
        ReadTransactions();
        //Add the transaction to the list and serialize
        transactionListContainer.transactionList.Add(newTransaction);
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(transactionListContainer, Formatting.Indented));
    }

    public void ReadTransactions()
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

    public void RemoveDuplicate()
    {
        ReadTransactions();
        //Does the transaction ID already exist in the list
        if (transactionListContainer.transactionList.Exists(x => x.GetTransactionID() == newTransaction.GetTransactionID()))
        {
            //Get the existing transacation and remove it from the list
            Transaction removeTrans = transactionListContainer.transactionList.Single(x => x.GetTransactionID() == newTransaction.GetTransactionID());
            transactionListContainer.transactionList.Remove(removeTrans);
        }

    }

    public List<Transaction> GetAllTransactions()
    {
        ReadTransactions();
        return transactionListContainer.transactionList;
    }

    public float NextTransactionID()
    { //This function returns the next available Transaction ID by loading all the saved transaction and getting the next
        //available transaction ID. 

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
