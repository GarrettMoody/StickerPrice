using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine;
using System.Globalization;

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

    private void WriteTransactionsToFile()
    { 
        fileUtility.ClearFile(filePath);
        fileUtility.WriteJson(filePath, JsonConvert.SerializeObject(transactionListContainer, Formatting.Indented));
    }

    /* Return sorted transaction list by date */
    public List<TransactionSummaryData> GetTransactionsSortedByDate()
    {
        List<Transaction> transactionList = transactionListContainer.transactionList;
        List<TransactionSummaryData> transactionSummaryDataList = new List<TransactionSummaryData>();
        Dictionary<string, TransactionSummaryData> transactionSummaryDataDict = new Dictionary<string, TransactionSummaryData>();
        Dictionary<string, TransactionSummarySubDetailsData> transactionSummarySubDetailsDataDict = new Dictionary<string, TransactionSummarySubDetailsData>();
        TransactionSummaryData transactionSummaryData;
        TransactionSummaryDetailsData transactionSummaryDetailsData = null;

        foreach (Transaction transaction in transactionList)
        {

            string date = transaction.datetime.Substring(0, 11); ;

            transactionSummaryDetailsData = null;

            if (transactionSummaryDataDict.ContainsKey(date))
            {
                transactionSummaryData = transactionSummaryDataDict[date];

                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    string owner = itemRowData.productOwner;

                    if (transactionSummarySubDetailsDataDict.ContainsKey(date + owner))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                            Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                            tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                            transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        }
                        else
                        {
                            transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        }
                        tempTransSubData.SetTotalPrice(tempTransSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData tempTransSubData = new TransactionSummarySubDetailsData(owner);
                        tempTransSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);

                        transactionSummarySubDetailsDataDict.Add(date + owner, tempTransSubData);
                    }
                }
            }
            else
            {
                transactionSummaryData = new TransactionSummaryData(date);

                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    string owner = itemRowData.productOwner;

                    if (transactionSummarySubDetailsDataDict.ContainsKey(date + owner))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                            Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                            tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                            transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        }
                        else
                        {
                            transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        }

                        tempTransSubData.SetTotalPrice(tempTransSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                       
                        transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData tempTransSubData = new TransactionSummarySubDetailsData(owner);
                        tempTransSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);

                        transactionSummarySubDetailsDataDict.Add(date + owner, tempTransSubData);
                        transactionSummaryDataDict.Add(date, transactionSummaryData);
                    }
                }
            }
        }

        transactionSummaryDataList = transactionSummaryDataDict.Values.ToList().OrderByDescending(transactionSummaryData2 => DateTime.ParseExact(transactionSummaryData2.primaryKey, "dd MMM yyyy", null)).ToList();

        return transactionSummaryDataList;
    }

    /* Return sorted transaction list by owner */
    public List<TransactionSummaryData> GetTransactionsSortedByOwner()
    {
        List<Transaction> transactionList = transactionListContainer.transactionList;
        List<TransactionSummaryData> transactionSummaryDataList = new List<TransactionSummaryData>();
        Dictionary<string, TransactionSummaryData> transactionSummaryDataDict = new Dictionary<string, TransactionSummaryData>();
        Dictionary<string, TransactionSummarySubDetailsData> transactionSummarySubDetailsDataDict = new Dictionary<string, TransactionSummarySubDetailsData>();
        TransactionSummaryData transactionSummaryData;
        TransactionSummaryDetailsData transactionSummaryDetailsData = null;

        foreach (Transaction transaction in transactionList)
        {

            string date = transaction.datetime.Substring(0, 11); ;

            transactionSummaryDetailsData = null;

            foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
            {
                string owner = itemRowData.productOwner;

                if (transactionSummaryDataDict.ContainsKey(owner))
                {
                    transactionSummaryData = transactionSummaryDataDict[owner];

                    if (transactionSummarySubDetailsDataDict.ContainsKey(owner + date))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[owner + date];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                            Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                            tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                            transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        }
                        else
                        {
                            transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        }
                        tempTransSubData.SetTotalPrice(tempTransSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + itemRowData.itemPrice * itemRowData.quantity);

                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData tempTransSubData = new TransactionSummarySubDetailsData(date);
                        tempTransSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);

                        transactionSummarySubDetailsDataDict.Add(owner + date , tempTransSubData);
                    }
                }
                else
                {
                    transactionSummaryData = new TransactionSummaryData(owner);

                    if (transactionSummarySubDetailsDataDict.ContainsKey(owner + date))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[owner + date];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                            Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                            tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                            transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        }
                        else
                        {
                            transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        }

                        tempTransSubData.SetTotalPrice(tempTransSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData tempTransSubData = new TransactionSummarySubDetailsData(date);
                        tempTransSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);

                        transactionSummarySubDetailsDataDict.Add(owner + date, tempTransSubData);
                        transactionSummaryDataDict.Add(owner, transactionSummaryData);
                    }
                }
            }
        }

        transactionSummaryDataList = transactionSummaryDataDict.Values.ToList().OrderByDescending(transactionSummaryData2 => transactionSummaryData2.primaryKey).ToList();

        return transactionSummaryDataList;
    }

    private void ReadTransactions()
    {
        transactionListContainer = JsonUtility.FromJson<TransactionListContainer>(fileUtility.ReadJson(filePath));
        
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
