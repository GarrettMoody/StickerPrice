using System;

[Serializable]
public class Transaction 
{
    public float transactionID;
    public ItemListData itemListData;
    public string paymentMethod;
    public string datetime;

    public const string CASH_PAYMENT_METHOD = "Cash";
    public const string CHECK_PAYMENT_METHOD = "Check";
    public const string CARD_PAYMENT_METHOD = "Card";

    public Transaction ()
    {
        itemListData = new ItemListData();
    }
    
    public Transaction(float transactionID)
    {
        this.transactionID = transactionID;
        itemListData = new ItemListData();
    }

    public Transaction(float transactionID, ItemListData itemListData, string paymentMethod, string datetime)
    {
        this.transactionID = transactionID;
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
