using UnityEngine;
using UnityEngine.UI;

public class CashRegisterPanel : MonoBehaviour
{
    private Text totalAmount;
    private Text itemAmount;
    private InputField cashReceived;
    private InputField cashBack;
    private ToggleGroup receiptSelection;
    private Toggle noneToggle;
    private Toggle bothToggle;
    private Toggle printToggle;
    private Toggle emailToggle;
    private Button completeButton;





    public Text GetTotalAmount()
    {
        return totalAmount;
    }

    public void SetTotalAmount(Text value)
    {
        totalAmount = value;
    }

    public Text GetItemAmount()
    {
        return itemAmount;
    }

    public void SetItemAmount(Text value)
    {
        itemAmount = value;
    }

    public string GetCashReceived()
    {
        return cashReceived.text;
    }

    public void SetCashReceived(InputField value)
    {
        cashReceived = value;
    }

    public string GetCashBack()
    {
        return cashBack.text;
    }

    public void SetCashBack(InputField value)
    {
        cashBack = value;
    }

    public ToggleGroup GetReceiptSelection()
    {
        return receiptSelection;
    }

}
