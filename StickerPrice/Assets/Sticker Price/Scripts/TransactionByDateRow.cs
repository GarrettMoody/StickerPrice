using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransactionByDateRow : ContentRow, IPointerClickHandler
{

    public Button expandButton;
    public Text transactionDate;
    public GameObject titlePanel;
    public Transaction transaction;

    private TransactionByDateList transactionByDateList;

    public new void Awake()
    {
        base.Awake();
        transactionByDateList = GetComponentInParent<TransactionByDateList>();
    }

    public void InitiateTransactionByDateRow(Transaction transaction)
    {
        this.transaction = transaction;
        transactionDate.text = transaction.datetime;
    }

    public override void OnDefaultButtonClick()
    {
        throw new System.NotImplementedException();
    }
}
