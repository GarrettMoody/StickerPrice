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

        //string filePath2 = "Assets/Sticker Price/Data Files/Temp33.json";
        //fileUtility.WriteJson(filePath2, JsonConvert.SerializeObject(transactionListContainer, Formatting.Indented));

        //List<Transaction> transactionList = transactionListContainer.transactionList;
        //int i = 1;
        //foreach (Transaction transaction in transactionList)
        //{
        //    Debug.Log("transaction.id>>>>" + transaction.transactionID);
        //    Debug.Log("transacion>>>>" + JsonConvert.SerializeObject(transaction, Formatting.Indented));
        //    Debug.Log("transaction.datetime>>>>" + (transaction.datetime == null ? "null" : transaction.datetime));

        //    DateTime transactionDateTime;
        //    //DateTime.TryParse(transaction.datetime, CultureInfo.CurrentUICulture, DateTimeStyles.AssumeLocal, out transactionDateTime);

        //    //transactionDateTime = DateTime.ParseExact(transaction.datetime, "d MMM yyyy h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

        //    string date = transaction.datetime.Substring(0, 11); ;
        //    string time = transaction.datetime.Substring(11);

        //    //string date = transactionDateTime.ToString("d MMM yyyy");
        //    //string time = transactionDateTime.ToString("hh:mm tt");


        //    Debug.Log("date>>>>" + date);
        //    Debug.Log("time>>>>" + time);
        //    Debug.Log("End>>>>" + i);
        //    i++;
        //    List<ItemRowData> itemRowDataList = transaction.itemListData.itemRowDataListContainer.itemRowDataList;

        //    foreach (ItemRowData itemRowData in itemRowDataList)
        //    {
        //        string owner = itemRowData.productOwner;
        //    }
        //}
    }

    private void WriteTransactionsToFile()
    { 
        fileUtility.ClearFile(filePath);
        fileUtility.WriteJson(filePath, JsonConvert.SerializeObject(transactionListContainer, Formatting.Indented));
    }

    /* Return sorted transaction list by date */
    public List<TransactionSummaryData> GetTransactionsSortedByDateTemp()
    {
        List<Transaction> transactionList = transactionListContainer.transactionList;
        List<TransactionSummaryData> transactionSummaryDataList = new List<TransactionSummaryData>();
        Dictionary<string, TransactionSummaryData> transactionSummaryDataDict = new Dictionary<string, TransactionSummaryData>();
        Dictionary<string, TransactionSummarySubDetailsData> transactionSummarySubDetailsDataDict = new Dictionary<string, TransactionSummarySubDetailsData>();
        TransactionSummaryData transactionSummaryData;
        int i = 1;
        foreach (Transaction transaction in transactionList)
        {
            Debug.Log("i>>>>" + i);
            i++;
            //DateTime transactionDateTime = DateTime.ParseExact(transaction.datetime, "dd MMM yyyy hh:mm tt", null);

            string date = transaction.datetime.Substring(0, 11); ;
            string time = transaction.datetime.Substring(11);
            //string date = transactionDateTime.ToString("dd MMM yyyy");
            //string time = transactionDateTime.ToString("hh:mm tt");
            Transaction tempTransaction;

            Debug.Log("transaction.id>>>>" + transaction.transactionID);
            Debug.Log("transaction.datetime>>>>" + transaction.datetime);
            Debug.Log("date>>>>" + date);
            Debug.Log("transactionSummaryDataDict.ContainsKey(date)>>>>" + transactionSummaryDataDict.ContainsKey(date));

            if (transactionSummaryDataDict.ContainsKey(date))
            {
                TransactionSummaryData tempTransData = transactionSummaryDataDict[date];

                TransactionSummaryDetailsData transactionSummaryDetailsData = null;
                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    string owner = itemRowData.productOwner;
                    if (transactionSummarySubDetailsDataDict.ContainsKey(date+owner))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        }

                        tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        tempTransData.getTransactionSummarySubDetailsData().Add(tempTransSubData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData transactionSummarySubDetailsData = new TransactionSummarySubDetailsData(itemRowData.productOwner);
                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");

                        tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        transactionSummarySubDetailsData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummarySubDetailsDataDict.Add(date + owner, transactionSummarySubDetailsData);
                        tempTransData.getTransactionSummarySubDetailsData().Add(transactionSummarySubDetailsData);
                    }
                }
            }
            else
            {
                TransactionSummaryDetailsData transactionSummaryDetailsData = null;
                transactionSummaryData = new TransactionSummaryData(date);


                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    string owner = itemRowData.productOwner;

                    Debug.Log("date + owner>>>>" + date + owner);
                    Debug.Log("transactionSummarySubDetailsDataDict.ContainsKey(date + owner)>>>>" + transactionSummarySubDetailsDataDict.ContainsKey(date + owner));

                    if (transactionSummarySubDetailsDataDict.ContainsKey(date + owner))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");
                        }

                        tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        tempTransSubData.SetTotalPrice(transactionSummaryDetailsData.GetTotalPrice() + (itemRowData.itemPrice * itemRowData.quantity));
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);
                    }
                    else
                    {
                        TransactionSummarySubDetailsData tempTransSubData = new TransactionSummarySubDetailsData(owner);
                        transactionSummaryDetailsData = new TransactionSummaryDetailsData(transaction.GetTransactionID() + "");

                        tempTransaction = new Transaction(transaction.GetTransactionID());
                        tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList.Add(itemRowData);
                        transactionSummaryDetailsData.getTransactionList().Add(tempTransaction);
                        tempTransSubData.SetTotalPrice(itemRowData.itemPrice * itemRowData.quantity);
                        tempTransSubData.getTransactionSummaryDetailsData().Add(transactionSummaryDetailsData);
                        transactionSummarySubDetailsDataDict.Add(date + owner, tempTransSubData);
                        transactionSummaryData.getTransactionSummarySubDetailsData().Add(tempTransSubData);

                        transactionSummaryDataDict.Add(date, transactionSummaryData);
                    }
                }
            }
        }

        foreach (TransactionSummaryData tsd in transactionSummaryDataDict.Values.ToList())
        {
            Debug.Log("Date>>>>" + tsd.GetPrimaryKey());
            foreach (TransactionSummarySubDetailsData tssd in tsd.getTransactionSummarySubDetailsData())
            {
                Debug.Log("Name>>>>" + tssd.GetPrimaryKey());
                Debug.Log("Total Price>>>>" + tssd.GetTotalPrice());
                foreach (TransactionSummaryDetailsData tsdd in tssd.getTransactionSummaryDetailsData())
                {
                    Debug.Log("Transaction Id>>>>" + tsdd.GetPrimaryKey());

                    foreach (Transaction trn in tsdd.getTransactionList())
                    {
                        Debug.Log("Transaction Id2>>>>" + trn);
                        filePath = "Assets/Sticker Price/Data Files/Temp.json";

                        fileUtility.ClearFile(filePath);
                        fileUtility.WriteJson(filePath, JsonConvert.SerializeObject(trn, Formatting.Indented));
                    }
                }
            }
        }

        transactionSummaryDataList = transactionSummaryDataDict.Values.ToList().OrderByDescending(transactionSummaryData2 => DateTime.ParseExact(transactionSummaryData2.primaryKey, "dd MMM yyyy", null)).ToList();

        return transactionSummaryDataList;
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
        int i = 1;

        foreach (Transaction transaction in transactionList)
        {
            //Debug.Log("i>>>>" + i);
            //DateTime transactionDateTime = DateTime.ParseExact(transaction.datetime, "dd MMM yyyy hh:mm tt", null);

            string date = transaction.datetime.Substring(0, 11); ;
            //string time = transaction.datetime.Substring(11);
            //string date = transactionDateTime.ToString("dd MMM yyyy");
            //string time = transactionDateTime.ToString("hh:mm tt");

            Debug.Log("transaction.id>>>>" + transaction.transactionID);
            //Debug.Log("transaction.datetime>>>>" + transaction.datetime);
            //Debug.Log("date>>>>" + i + ">>>>" + date);
            //Debug.Log("transactionSummaryDataDict.Count>>>>" + transactionSummaryDataDict.Count);
            //Debug.Log("transactionSummaryDataDict.ContainsKey(date)>>>>" + transactionSummaryDataDict.ContainsKey(date));
            i++;
            transactionSummaryDetailsData = null;
            int j = 0;

            if (transactionSummaryDataDict.ContainsKey(date))
            {
                transactionSummaryData = transactionSummaryDataDict[date];
                //Debug.Log("date2>>>>" + date);

                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    Debug.Log("transaction.id>>>>"+j+">>" + transaction.transactionID);
                    string owner = itemRowData.productOwner;

                    Debug.Log("date + owner1>>>>" + date + ">>" + owner);
                    Debug.Log("transactionSummarySubDetailsDataDict.ContainsKey(date + owner)>>"+ j + ">>" + transactionSummarySubDetailsDataDict.ContainsKey(date + owner));
                    j++;

                    if (transactionSummarySubDetailsDataDict.ContainsKey(date + owner))
                    {
                        Debug.Log(j+">>if");
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            Debug.Log(j+">>null");
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
                        Debug.Log("else");
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
                //Debug.Log("date3>>>>" + date);

                foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
                {
                    string owner = itemRowData.productOwner;

                    //Debug.Log("date + owner2>>>>" + date +">>"+ owner);
                    //Debug.Log("transactionSummarySubDetailsDataDict.ContainsKey(date + owner)2>>>>" + transactionSummarySubDetailsDataDict.ContainsKey(date + owner));

                    if (transactionSummarySubDetailsDataDict.ContainsKey(date + owner))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[date + owner];

                        if (transactionSummaryDetailsData == null)
                        {
                            Debug.Log(j + ">>null");
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

        foreach (TransactionSummaryData tsd in transactionSummaryDataDict.Values.ToList())
        {
            Debug.Log("Date>>>>" + tsd.GetPrimaryKey());
            Debug.Log("TransactionSummaryData>>>>" + JsonConvert.SerializeObject(tsd, Formatting.Indented));

            foreach (TransactionSummarySubDetailsData tssd in tsd.getTransactionSummarySubDetailsData())
            {
                Debug.Log("TransactionSummarySubDetailsData>>>>" + JsonConvert.SerializeObject(tssd, Formatting.Indented));
                Debug.Log("Total Price>>>>" + tssd.GetTotalPrice());
                foreach (TransactionSummaryDetailsData tsdd in tssd.getTransactionSummaryDetailsData())
                {
                    //Debug.Log("Transaction Id>>>>" + tsdd.GetPrimaryKey());

                    foreach (Transaction trn in tsdd.getTransactionList())
                    {
                        Debug.Log("Transaction Id>>>>" + JsonConvert.SerializeObject(trn, Formatting.Indented));
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
        int i = 1;

        foreach (Transaction transaction in transactionList)
        {
            //Debug.Log("i>>>>" + i);
            //DateTime transactionDateTime = DateTime.ParseExact(transaction.datetime, "dd MMM yyyy hh:mm tt", null);

            string date = transaction.datetime.Substring(0, 11); ;
            //string time = transaction.datetime.Substring(11);
            //string date = transactionDateTime.ToString("dd MMM yyyy");
            //string time = transactionDateTime.ToString("hh:mm tt");

            Debug.Log("transaction.id>>>>" + transaction.transactionID);
            //Debug.Log("transaction.datetime>>>>" + transaction.datetime);
            //Debug.Log("date>>>>" + i + ">>>>" + date);
            //Debug.Log("transactionSummaryDataDict.Count>>>>" + transactionSummaryDataDict.Count);
            //Debug.Log("transactionSummaryDataDict.ContainsKey(date)>>>>" + transactionSummaryDataDict.ContainsKey(date));
            i++;
            transactionSummaryDetailsData = null;
            int j = 0;

            foreach (ItemRowData itemRowData in transaction.itemListData.itemRowDataListContainer.itemRowDataList)
            {
                string owner = itemRowData.productOwner;

                if (transactionSummaryDataDict.ContainsKey(owner))
                {
                    transactionSummaryData = transactionSummaryDataDict[owner];
                    //Debug.Log("date2>>>>" + date);

                    Debug.Log("transaction.id>>>>" + j + ">>" + transaction.transactionID);
                    Debug.Log("date + owner1>>>>" + date + ">>" + owner);
                    Debug.Log("transactionSummarySubDetailsDataDict.ContainsKey(date + owner)>>" + j + ">>" + transactionSummarySubDetailsDataDict.ContainsKey(date + owner));
                    j++;

                    if (transactionSummarySubDetailsDataDict.ContainsKey(owner + date))
                    {
                        Debug.Log(j + ">>if");
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[owner + date];

                        if (transactionSummaryDetailsData == null)
                        {
                            Debug.Log(j + ">>null");
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
                        Debug.Log("else");
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
                    //Debug.Log("date3>>>>" + date);
                    
                    //Debug.Log("date + owner2>>>>" + date +">>"+ owner);
                    //Debug.Log("transactionSummarySubDetailsDataDict.ContainsKey(date + owner)2>>>>" + transactionSummarySubDetailsDataDict.ContainsKey(date + owner));

                    if (transactionSummarySubDetailsDataDict.ContainsKey(owner + date))
                    {
                        TransactionSummarySubDetailsData tempTransSubData = transactionSummarySubDetailsDataDict[owner + date];

                        if (transactionSummaryDetailsData == null)
                        {
                            Debug.Log(j + ">>null");
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
        i = 0;
        foreach (TransactionSummaryData tsd in transactionSummaryDataDict.Values.ToList())
        {

            Debug.Log("Name>>>>" + i + ">>>" + tsd.GetPrimaryKey());
            Debug.Log("TransactionSummaryData>>>>" + i + ">>>" + JsonConvert.SerializeObject(tsd, Formatting.Indented));

            foreach (TransactionSummarySubDetailsData tssd in tsd.getTransactionSummarySubDetailsData())
            {
                Debug.Log("TransactionSummarySubDetailsData>>>>" + i + ">>" + JsonConvert.SerializeObject(tssd, Formatting.Indented));
                Debug.Log("Total Price>>>>" + tssd.GetTotalPrice());
                foreach (TransactionSummaryDetailsData tsdd in tssd.getTransactionSummaryDetailsData())
                {
                    //Debug.Log("Transaction Id>>>>" + tsdd.GetPrimaryKey());

                    foreach (Transaction trn in tsdd.getTransactionList())
                    {
                        Debug.Log("Transaction Id>>>>" + i + ">>"  + JsonConvert.SerializeObject(trn, Formatting.Indented));
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
