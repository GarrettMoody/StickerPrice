using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionReceiptPanel : MonoBehaviour
{

    public ItemList itemList;
    public Text transactionNumber;
    public Text transactionDateTime;
    public Text paymentMethod;

    private Transaction transaction;

    public void SetTransactionNumber(string transactionNumber)
    {
        this.transactionNumber.text = transactionNumber;
    }

    public void SetPaymentMethod(string paymentMethod)
    {
        this.paymentMethod.text = paymentMethod;
    }

    public void SetTransactionDateTime(string transactionDateTime)
    {
        this.transactionDateTime.text = transactionDateTime;
    }

    public void SetTransaction(Transaction transaction)
    {
        this.transaction = transaction;
    }

    public Transaction GetTransaction()
    {
        return this.transaction;
    }
}