using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CheckoutPanel : MonoBehaviour {

    public ItemList itemList;
    public Toggle cashToggle;
    public Toggle checkToggle;
    public Toggle creditToggle;
    public ToggleGroup paymentMethodToggleGroup;
    public GameObject cashRegisterPanel;
    public Text transactionNumber;
    public Text currentDateTime;

    [SerializeField, HideInInspector] private Transaction transaction;

    public void Update()
    {
        currentDateTime.text = System.DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
    }

    public void CheckoutButtonOnClickListener()
    {
        Toggle selectedToggle = paymentMethodToggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle != null && selectedToggle == cashToggle)
        {
            SaveTransaction(Transaction.CASH_PAYMENT_METHOD);
            this.gameObject.SetActive(false);
            cashRegisterPanel.gameObject.SetActive(true);

        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void SetTransactionNumber(string transactionNumber)
    {
        this.transactionNumber.text = transactionNumber;
    }

    public void SetTransaction(Transaction transaction)
    {
        this.transaction = transaction;
    }

    public Transaction GetTransaction()
    {
        return this.transaction;
    }

    private void SaveTransaction(string paymentMethod)
    {
        transaction.SetDateTime(currentDateTime.text);
        transaction.SetItemListData(itemList.itemListData);
        transaction.SetPaymentMethod(paymentMethod);

        TransactionData transactionData = new TransactionData();
        transactionData.WriteTransaction(transaction);
    }
}
