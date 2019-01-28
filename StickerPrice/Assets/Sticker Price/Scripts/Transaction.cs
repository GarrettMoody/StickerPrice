using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Transaction 
{
    [SerializeField] public float transactionID;
    [SerializeField] public ItemListData itemListData;
    [SerializeField] public string paymentMethod;
    [SerializeField] public string datetime;

    private TransactionData transactionData;

    public const string CASH_PAYMENT_METHOD = "Cash";
    public const string CHECK_PAYMENT_METHOD = "Check";
    public const string CARD_PAYMENT_METHOD = "Card";
    
    public Transaction()
    {
        transactionData = new TransactionData();
        transactionID = transactionData.NextTransactionID();
        itemListData = new ItemListData();
    }

    public Transaction(ItemListData itemListData, string paymentMethod, string datetime)
    {
        transactionData = new TransactionData();
        this.transactionID = transactionData.NextTransactionID();
        this.itemListData = itemListData;
        this.paymentMethod = paymentMethod;
       
        this.datetime = datetime;
    }

    public float GetTransactionID()
    {
        return transactionID;
    }

    public void SetItemListData(ItemListData itemListData)
    {
        this.itemListData = itemListData;
    }

    public void SetPaymentMethod(string method)
    {
        paymentMethod = method;
    }

    public void SetDateTime(string datetime)
    {
        this.datetime = datetime;
        
    }
}
