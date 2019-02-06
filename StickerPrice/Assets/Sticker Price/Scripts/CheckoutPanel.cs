using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutPanel : MonoBehaviour {

    public ItemList itemList;
    public Toggle cashToggle;
    public Toggle checkToggle;
    public Toggle creditToggle;
    public ToggleGroup paymentMethodToggleGroup;
    public CashRegisterPanel cashRegisterPanel;
    public Text transactionNumber;
    public Text currentDateTime;

    private Transaction transaction;

    public void Update()
    {
        currentDateTime.text = System.DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
    }

    public void CheckoutButtonOnClickListener()
    {
        Toggle selectedToggle = paymentMethodToggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedToggle != null && selectedToggle == cashToggle)
        {
            transaction.paymentMethod = Transaction.CASH_PAYMENT_METHOD;
            transaction.datetime = currentDateTime.text;
            this.gameObject.SetActive(false);
            cashRegisterPanel.OpenCashRegisterPanel(transaction);
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
}
