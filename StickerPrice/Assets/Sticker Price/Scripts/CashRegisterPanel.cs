using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CashRegisterPanel : MonoBehaviour
{
    public TextMeshProUGUI totalAmount;
    public TextMeshProUGUI itemAmount;
    public InputField cashReceived;
    public InputField cashBack;
    public ToggleGroup receiptSelection;
    public Toggle noneToggle;
    public Toggle bothToggle;
    public Toggle printToggle;
    public Toggle emailToggle;
    public Button completeButton;
    public ScanPanel scanPanel;

    private float cashReceivedValue;
    private Transaction transaction;

    //Constants
    readonly Color32 THEME_GREEN = new Color32(0x5C, 0xAB, 0x40, 0xFF);
    readonly Color32 RED = new Color32(0xE2, 0x23, 0x1A, 0xFF);
    readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    readonly Color32 DARK_GREY = new Color32(0x52, 0x53, 0x49, 0xFF);

    public void OpenCashRegisterPanel(Transaction transaction)
    {
        this.gameObject.SetActive(true);
        this.transaction = transaction;
        SetItemAmount(transaction.itemListData.itemTotal.ToString());
        SetTotalAmount(transaction.itemListData.priceTotal.ToString());
        SetCashReceivedValue(0);
        SetCashBack();
    }

    public string GetTotalAmount()
    {
        return totalAmount.text;
    }

    public void SetTotalAmount(string value)
    {
        totalAmount.text = string.Format("{0:C2}", float.Parse(value));
    }

    public string GetItemAmount()
    {
        return itemAmount.text;
    }

    public void SetItemAmount(string value)
    {
        itemAmount.text = value;
    }

    public string GetCashReceived()
    {
        return cashReceived.text;
    }

    public void SetCashReceived(string value)
    {
        cashReceived.text = string.Format("{0:C2}", float.Parse(value));
    }

    public string GetCashBack()
    {
        return cashBack.text;
    }

    public void SetCashBack()
    {
        //Cash Back Value = Cash Received - Total Amount
        float cashBackValue = float.Parse(GetCashReceived(), System.Globalization.NumberStyles.Currency) - float.Parse(GetTotalAmount(), System.Globalization.NumberStyles.Currency);
        cashBack.text = string.Format("{0:C2}", cashBackValue);
    }

    public void SetCashReceivedValue(float value)
    {
        cashReceivedValue = value;
        float cashRecieved = cashReceivedValue / 100;
        SetCashReceived(cashRecieved.ToString());
        SetCashBack();
    }

    public float GetCashReceivedValue()
    {
        return cashReceivedValue;
    }

    public void OnNumberButtonDown()
    {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = THEME_GREEN;
        button.GetComponentInChildren<Text>().color = WHITE;
    }

    public void OnNumberButtonUp()
    {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = WHITE;
        button.GetComponentInChildren<Text>().color = THEME_GREEN;
    }

    public void OnNumberButtonPress(int number)
    {
        SetCashReceivedValue(float.Parse(GetCashReceivedValue().ToString() + number.ToString()));
    }

    public void OnDoubleZeroButtonPress()
    {
        SetCashReceivedValue(float.Parse(GetCashReceivedValue().ToString() + "00"));
    }

    public void OnClearButtonPress()
    {
        SetCashReceivedValue(0);
    }

    private void SaveTransaction()
    {
        TransactionData transactionData = new TransactionData();
        transactionData.AddTransaction(transaction);
    }

    public void OnCompleteButtonOnClickListener()
    {
        SaveTransaction();
        this.gameObject.SetActive(false);
        scanPanel.gameObject.SetActive(true);
        scanPanel.StartNewTransaction();
    }

    public void SetTransaction(Transaction transaction)
    {
        this.transaction = transaction;
    }
}
