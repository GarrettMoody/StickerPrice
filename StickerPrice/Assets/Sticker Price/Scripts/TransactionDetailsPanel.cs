using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionDetailsPanel : MonoBehaviour
{
    public TransactionDetailsRow transactionDetailsRowPrefab;
    public TransactionDetailsList transactionDetailsList;
    List<TransactionSummaryData> transactionSummaryDataList;
    Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict;
    public string selectedMainPanelKey = "";
    public bool isOwnerSelected = false;
    public GameObject headerPanelDate;
    public GameObject headerPanelOwner;
    public TransactionSummarySubDetailsData selectedMainPanelSubDetailsData;
    public TransactionReceiptPanel transactionReceiptPanel;

    public new void Awake()
    {
        //LoadTransactionListFromFile();
    }

    public void LoadTransactionListFromFile()
    {
        transactionGameObjectDict = new Dictionary<string, TransactionSummaryGameObjectList>();
        TransactionData transactionData = new TransactionData();
        string[] splitMainePanelName = selectedMainPanelKey.Split('_');

        if (isOwnerSelected)
        {
            transactionSummaryDataList = transactionData.GetTransactionsSortedByOwner();
            headerPanelDate.GetComponent<Text>().text = "Transaction Date: "+ splitMainePanelName[1] + " " + splitMainePanelName[2] + " " + splitMainePanelName[3];
            headerPanelOwner.GetComponent<Text>().text = "Owner: "+ splitMainePanelName[0];
        }
        else
        {
            transactionSummaryDataList = transactionData.GetTransactionsSortedByDate();
            headerPanelDate.GetComponent<Text>().text = "Transaction Date: " + splitMainePanelName[0] + " " + splitMainePanelName[1] + " " + splitMainePanelName[2];
            headerPanelOwner.GetComponent<Text>().text = "Owner: " + splitMainePanelName[3];
        }

        displayTransactionDetails(false, "");
    }

    public void displayTransactionDetails(bool isAlreadyExpanded, string selectedPanelKey)
    {
        transactionDetailsList.RemoveAllRows();
        TransactionDetailsRow row = transactionDetailsList.AddRow();
        TransactionSummaryGameObjectList transactionGameObjects = null;

        row.InitiateTransactionDetailsRow(selectedMainPanelKey);
        transactionGameObjectDict = row.expandCollapseTransactionDetailsRows(selectedMainPanelSubDetailsData, selectedMainPanelKey,
            true, transactionGameObjectDict);
        transactionGameObjects = transactionGameObjectDict[selectedMainPanelKey];
        GameObject transactionDetailsParentPanel = transactionGameObjects.GetParentGameObject();

        foreach (TransactionSummaryDetailsData tsDetailsData in selectedMainPanelSubDetailsData.getTransactionSummaryDetailsData())
        {
            string currentTransactionDetailsPanelkey = selectedMainPanelKey + "_" + tsDetailsData.GetPrimaryKey();
            bool curTransactionPanelExpandedFlag = false;

            if ((currentTransactionDetailsPanelkey).Equals(selectedPanelKey))
            {
                curTransactionPanelExpandedFlag = !isAlreadyExpanded;
            }
            else
            {
                if (transactionGameObjectDict.ContainsKey(currentTransactionDetailsPanelkey))
                {
                    transactionGameObjects = transactionGameObjectDict[currentTransactionDetailsPanelkey];
                    curTransactionPanelExpandedFlag = transactionGameObjects.IsExpanded();

                    if (curTransactionPanelExpandedFlag)
                    {
                        //transactionGameObjectDict = row.ExpandCollapseThirdPanel(curFinalPanelExpandedFlag, tsSubDetailsData, currentSecondSubPanelkey, transactionGameObjectDict);
                    }
                }
            }

            GameObject transactionDetailsPanel = transactionDetailsParentPanel.transform.Find("TransactionsPanel_" + currentTransactionDetailsPanelkey).gameObject;
            //transactionDetailsPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => displayTransactionDetails(curTransactionPanelExpandedFlag, currentTransactionDetailsPanelkey));
            transactionDetailsPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => openTransactionReceipt(tsDetailsData));

        }
    }

    public void openTransactionReceipt(TransactionSummaryDetailsData tsDetailsData)
    {
        this.gameObject.SetActive(false);
        transactionReceiptPanel.gameObject.SetActive(true);

        Transaction tempTransaction = tsDetailsData.getTransactionList()[0];
        
        //Rebuild the list of rows in the checkout panel
        transactionReceiptPanel.itemList.RemoveAllRows();
        foreach (ItemRowData rowData in tempTransaction.itemListData.itemRowDataListContainer.itemRowDataList)
        {
            ItemRow newRow = transactionReceiptPanel.itemList.AddItem();
            newRow.SetScanString(rowData.scanString);
            newRow.SetItemDescription(rowData.itemDescription);
            newRow.SetProductOwner(rowData.productOwner);
            newRow.SetItemPrice(rowData.itemPrice);
            newRow.SetItemOriginalPrice(rowData.itemPrice);
            newRow.SetQuantity(rowData.quantity);
            newRow.UpdatePriceText();

            newRow.plusButton.gameObject.SetActive(false);
            newRow.minusButton.gameObject.SetActive(false);
            newRow.quantityInputField.readOnly = true;
            newRow.ellipsis.SetActive(false);
        }

        transactionReceiptPanel.itemList.CalculateTotals();

        //Send all data to the checkout panel
        //transactionReceiptPabel.itemList.SetItemTotal(this.itemList.GetItemTotal());
        //transactionReceiptPabel.itemList.SetPriceSubtotal(this.itemList.GetPriceSubtotal());
        //transactionReceiptPabel.itemList.SetTaxTotal(this.itemList.GetTaxTotal());
        transactionReceiptPanel.SetTransactionNumber(tsDetailsData.GetPrimaryKey());
        transactionReceiptPanel.SetPaymentMethod(tempTransaction.paymentMethod);
        transactionReceiptPanel.SetTransactionDateTime(tempTransaction.datetime);
        transactionReceiptPanel.SetTransaction(tempTransaction);
    }
}