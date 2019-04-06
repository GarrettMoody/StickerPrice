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
        Dictionary<string, TransactionSummaryFirstSubData> transactionSummaryFirstSubDataDict = new Dictionary<string, TransactionSummaryFirstSubData>();
        Dictionary<string, TransactionSummarySecondSubData> transactionSummarySecondSubDataDict = new Dictionary<string, TransactionSummarySecondSubData>();
        Dictionary<string, TransactionSummarySubDetailsData> transactionSummarySubDetailsDataDict = new Dictionary<string, TransactionSummarySubDetailsData>();

        TransactionSummaryData transactionSummaryData;
        TransactionSummaryFirstSubData transactionSummaryFirstSubData = null;
        TransactionSummarySecondSubData transactionSummarySecondSubData = null;
        TransactionSummarySubDetailsData transactionSummarySubDetailsData = null;
        TransactionSummaryDetailsData transactionSummaryDetailsData = null;

        foreach (Transaction transaction in transactionList)
        {
            string date = transaction.datetime.Substring(0, 11);
            string year = date.Substring(7, 4);
            string month = date.Substring(3, 3);
            string day = date.Substring(0, 2);
            string time = transaction.datetime.Substring(12, 8);
            transactionSummaryDetailsData = null;

            transactionSummaryData = new TransactionSummaryData(year);

            foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
            {
                string owner = itemRowData.productOwner;

                if (transactionSummarySubDetailsDataDict.ContainsKey(year + month + day + owner))
                {
                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                    transactionSummaryDetailsData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);

                    transactionSummarySubDetailsData = transactionSummarySubDetailsDataDict[year + month + day + owner];
                    transactionSummarySecondSubData = transactionSummarySecondSubDataDict[year + month + day];
                    transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[year + month];
                    transactionSummaryData = transactionSummaryDataDict[year];

                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);

                    transactionSummarySubDetailsData.SetTotalPrice(transactionSummarySubDetailsData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummarySecondSubData.SetTotalPrice(transactionSummarySecondSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                }
                else if (transactionSummarySecondSubDataDict.ContainsKey(year + month + day))
                {
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                    transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(owner);
                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySecondSubData = transactionSummarySecondSubDataDict[year + month + day];
                    transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[year + month];
                    transactionSummaryData = transactionSummaryDataDict[year];

                    transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);

                    transactionSummarySecondSubData.SetTotalPrice(transactionSummarySecondSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                    transactionSummarySubDetailsDataDict.Add(year + month + day + owner, transactionSummarySubDetailsData);
                }
                else if (transactionSummaryFirstSubDataDict.ContainsKey(year + month))
                {
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                    transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(owner);
                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySecondSubData = new TransactionSummarySecondSubData(day);
                    transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                    transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[year + month];
                    transactionSummaryData = transactionSummaryDataDict[year];

                    transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                    transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                    transactionSummarySubDetailsDataDict.Add(year + month + day + owner, transactionSummarySubDetailsData);
                    transactionSummarySecondSubDataDict.Add(year + month + day, transactionSummarySecondSubData);
                }
                else if(transactionSummaryDataDict.ContainsKey(year))
                {
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                    transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(owner);
                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySecondSubData = new TransactionSummarySecondSubData(day);
                    transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                    transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryFirstSubData = new TransactionSummaryFirstSubData(month);
                    transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                    transactionSummaryFirstSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryData = transactionSummaryDataDict[year];
                    transactionSummaryData.GetTransactionSummaryFirstSubDataList().Add(transactionSummaryFirstSubData);
                    transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                    transactionSummarySubDetailsDataDict.Add(year + month + day + owner, transactionSummarySubDetailsData);
                    transactionSummarySecondSubDataDict.Add(year + month + day, transactionSummarySecondSubData);
                    transactionSummaryFirstSubDataDict.Add(year + month, transactionSummaryFirstSubData);
                }
                else
                {
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                    transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(owner);
                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySecondSubData = new TransactionSummarySecondSubData(day);
                    transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                    transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryFirstSubData = new TransactionSummaryFirstSubData(month);
                    transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                    transactionSummaryFirstSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryData = new TransactionSummaryData(year);
                    transactionSummaryData.GetTransactionSummaryFirstSubDataList().Add(transactionSummaryFirstSubData);
                    transactionSummaryData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySubDetailsDataDict.Add(year + month + day + owner, transactionSummarySubDetailsData);
                    transactionSummarySecondSubDataDict.Add(year + month + day, transactionSummarySecondSubData);
                    transactionSummaryFirstSubDataDict.Add(year + month, transactionSummaryFirstSubData);
                    transactionSummaryDataDict.Add(year, transactionSummaryData);
                }
            }
        }

        transactionSummaryDataList = transactionSummaryDataDict.Values.ToList().OrderByDescending(transactionSummaryData2 => DateTime.ParseExact(transactionSummaryData2.primaryKey, "yyyy", null)).ToList();

        return transactionSummaryDataList;
    }

    /* Return sorted transaction list by owner */
    public List<TransactionSummaryData> GetTransactionsSortedByOwner()
    {
        List<Transaction> transactionList = transactionListContainer.transactionList;
        List<TransactionSummaryData> transactionSummaryDataList = new List<TransactionSummaryData>();
        Dictionary<string, TransactionSummaryData> transactionSummaryDataDict = new Dictionary<string, TransactionSummaryData>();
        Dictionary<string, TransactionSummaryFirstSubData> transactionSummaryFirstSubDataDict = new Dictionary<string, TransactionSummaryFirstSubData>();
        Dictionary<string, TransactionSummarySecondSubData> transactionSummarySecondSubDataDict = new Dictionary<string, TransactionSummarySecondSubData>();
        Dictionary<string, TransactionSummarySubDetailsData> transactionSummarySubDetailsDataDict = new Dictionary<string, TransactionSummarySubDetailsData>();
        TransactionSummaryData transactionSummaryData;
        TransactionSummaryFirstSubData transactionSummaryFirstSubData = null;
        TransactionSummarySecondSubData transactionSummarySecondSubData = null;
        TransactionSummarySubDetailsData transactionSummarySubDetailsData = null;
        TransactionSummaryDetailsData transactionSummaryDetailsData = null;

        foreach (Transaction transaction in transactionList)
        {
            string date = transaction.datetime.Substring(0, 11);
            string year = date.Substring(7, 4);
            string month = date.Substring(3, 3);
            string day = date.Substring(0, 2);
            string time = transaction.datetime.Substring(12, 8);

            transactionSummaryDetailsData = null;

            foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
            {
                string owner = itemRowData.productOwner;

                if (transactionSummaryDataDict.ContainsKey(owner))
                {
                    transactionSummaryData = transactionSummaryDataDict[owner];

                    if (transactionSummarySubDetailsDataDict.ContainsKey(owner + year + month + day))
                    {
                        transactionSummarySubDetailsData = transactionSummarySubDetailsDataDict[owner + year + month + day];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                            Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                            tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                            transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                            transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        }
                        else
                        {
                            transactionSummaryDetailsData.getTransactionList().First().itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        }
                        transactionSummaryDetailsData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.SetTransactionTime(time);
                        transactionSummarySubDetailsData.SetTotalPrice(transactionSummarySubDetailsData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                        transactionSummarySecondSubData = transactionSummarySecondSubDataDict[owner + year + month];
                        transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[owner + year];

                        transactionSummarySecondSubData.SetTotalPrice(transactionSummarySecondSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                    }
                    else if (transactionSummarySecondSubDataDict.ContainsKey(owner + year + month))
                    {
                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.SetTransactionTime(time);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                        transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(day);
                        transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummarySecondSubData = transactionSummarySecondSubDataDict[owner + year + month];
                        transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[owner + year];

                        transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                        transactionSummarySecondSubData.SetTotalPrice(transactionSummarySecondSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                        transactionSummarySubDetailsDataDict.Add(owner + year + month + day, transactionSummarySubDetailsData);
                    }
                    else if (transactionSummaryFirstSubDataDict.ContainsKey(owner + year))
                    {
                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.SetTransactionTime(time);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                        transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(day);
                        transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummarySecondSubData = new TransactionSummarySecondSubData(month);
                        transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                        transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummaryFirstSubData = transactionSummaryFirstSubDataDict[owner + year];

                        transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                        transactionSummaryFirstSubData.SetTotalPrice(transactionSummaryFirstSubData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                        transactionSummarySubDetailsDataDict.Add(owner + year + month + day, transactionSummarySubDetailsData);
                        transactionSummarySecondSubDataDict.Add(owner + year + month, transactionSummarySecondSubData);
                    }
                    else
                    {
                        Debug.Log("GetTransactionsSortedByOwner>>>>GetTransactionID>>" + transaction.GetTransactionID());
                        Debug.Log("GetTransactionsSortedByOwner>>>>day>>" + day);
                        Debug.Log("GetTransactionsSortedByOwner>>>>month>>" + month);
                        Debug.Log("GetTransactionsSortedByOwner>>>>year>>" + year);

                        Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.SetTransactionTime(time);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                        transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(day);
                        transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummarySecondSubData = new TransactionSummarySecondSubData(month);
                        transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                        transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummaryFirstSubData = new TransactionSummaryFirstSubData(year);
                        transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                        transactionSummaryFirstSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                        transactionSummaryData.GetTransactionSummaryFirstSubDataList().Add(transactionSummaryFirstSubData);
                        transactionSummaryData.SetTotalPrice(transactionSummaryData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));

                        transactionSummarySubDetailsDataDict.Add(owner + year + month + day, transactionSummarySubDetailsData);
                        transactionSummarySecondSubDataDict.Add(owner + year + month, transactionSummarySecondSubData);
                        transactionSummaryFirstSubDataDict.Add(owner + year, transactionSummaryFirstSubData);
                    }
                }
                else
                {
                    Transaction tempTransaction = new Transaction(transaction.GetTransactionID());
                    tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);

                    transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                    transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                    transactionSummaryDetailsData.SetTransactionTime(time);
                    transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);

                    transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(day);
                    transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                    transactionSummarySubDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySecondSubData = new TransactionSummarySecondSubData(month);
                    transactionSummarySecondSubData.GetTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                    transactionSummarySecondSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryFirstSubData = new TransactionSummaryFirstSubData(year);
                    transactionSummaryFirstSubData.GetTransactionSummarySecondSubDataList().Add(transactionSummarySecondSubData);
                    transactionSummaryFirstSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummaryData = new TransactionSummaryData(owner);
                    transactionSummaryData.GetTransactionSummaryFirstSubDataList().Add(transactionSummaryFirstSubData);
                    transactionSummaryData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);

                    transactionSummarySubDetailsDataDict.Add(owner + year + month + day, transactionSummarySubDetailsData);
                    transactionSummarySecondSubDataDict.Add(owner + year + month, transactionSummarySecondSubData);
                    transactionSummaryFirstSubDataDict.Add(owner + year, transactionSummaryFirstSubData);
                    transactionSummaryDataDict.Add(owner, transactionSummaryData);
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
