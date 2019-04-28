using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransactionDetailsRow : ContentRow, IPointerClickHandler
{
    private TransactionDetailsList parentTransactionList;
    public GameObject transactionsPanel;
    public GameObject mainPanel;

    public new void Awake()
    {
        base.Awake();
        parentTransactionList = GetComponentInParent<TransactionDetailsList>();
    }

    public void InitiateTransactionDetailsRow(string title)
    {
        transactionsPanel.SetActive(false);
    }

    public Dictionary<string, TransactionSummaryGameObjectList> expandCollapseTransactionDetailsRows(TransactionSummarySubDetailsData selectedTransSubData, string mainPanelKey,
        bool shouldExpandPanel, Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {
        if (!shouldExpandPanel)
        {
            transactionGameObjectDict.Remove(mainPanelKey);
        }
        else
        {
            transactionGameObjectDict.Remove(mainPanelKey);

            TransactionSummaryGameObjectList transactionGameObjects = new TransactionSummaryGameObjectList(mainPanelKey, mainPanel);
            transactionGameObjects.SetExpandedFlag(true);
            transactionGameObjectDict.Add(mainPanelKey, transactionGameObjects);

            foreach (TransactionSummaryDetailsData tsDetailsData in selectedTransSubData.getTransactionSummaryDetailsData())
            {
                GameObject tempTransactionsPanel = (GameObject)Instantiate(transactionsPanel, transform.position, transform.rotation);
                Transform[] ts = tempTransactionsPanel.transform.GetComponentsInChildren<Transform>();
                tempTransactionsPanel.name = "TransactionsPanel_" + mainPanelKey + "_" + tsDetailsData.GetPrimaryKey();

                foreach (Transform t in ts)
                {
                    switch (t.gameObject.name)
                    {
                        case "TransactionPanelMainText":
                            t.gameObject.GetComponent<Text>().text = "Transaction #" + tsDetailsData.GetPrimaryKey() + ", " + tsDetailsData.GetTransactionTime() + " - $" + tsDetailsData.GetTotalPrice();
                            break;
                        case "TransactionPanelSubText":
                            t.gameObject.GetComponent<Text>().text = "";
                            break;
                    }
                }

                tempTransactionsPanel.SetActive(true);
                tempTransactionsPanel.transform.SetParent(mainPanel.transform, false);
                transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempTransactionsPanel);
            }
        }
        return transactionGameObjectDict;
    }

    public override void OnDefaultButtonClick()
    {
        throw new System.NotImplementedException();
    }
}
