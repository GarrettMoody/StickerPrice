using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransactionByDateRow : ContentRow, IPointerClickHandler
{
    public bool isOrderByDate;

    public GameObject mainPanel;
    public GameObject titlePanel;
    public GameObject firstSubPanel;
    public GameObject secondSubPanel;
    public GameObject thirdSubPanel;
    public GameObject transactionsPanel;

    public Button transactionDetailsHiddenButton;
    public Button expandCollapseButton;

    public Text mainTitle;

    //public TransactionSummaryData transaction;
    private TransactionByDateList parentTransactionList;
    Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict;

    public new void Awake()
    {
        base.Awake();
        transactionGameObjectDict = new Dictionary<string, TransactionSummaryGameObjectList>();
        parentTransactionList = GetComponentInParent<TransactionByDateList>();
    }

    public void InitiateTransactionByDateRow(TransactionSummaryData transaction,bool isOrderByDate)
    {
        //mainTitle.text = transaction.primaryKey;
        //this.isOrderByDate = isOrderByDate;
        //firstSubPanel.SetActive(false);
        //secondSubPanel.SetActive(false);
        //thirdSubPanel.SetActive(false);
        //transactionsPanel.SetActive(false);
        //expandCollapseButton.GetComponentInChildren<Text>().text = ">";
    }

    public void InitiateTransactionRows(List<TransactionSummaryData> transactionList, bool isOrderByDate, Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {
        this.isOrderByDate = isOrderByDate;
        firstSubPanel.SetActive(false);
        secondSubPanel.SetActive(false);
        thirdSubPanel.SetActive(false);
        transactionsPanel.SetActive(false);
        TransactionSummaryGameObjectList transactionGameObjects = null;

        foreach (TransactionSummaryData tsData in transactionList)
        {
            GameObject tempTitlePanel = (GameObject)Instantiate(titlePanel, transform.position, transform.rotation);
            Transform[] ts = tempTitlePanel.transform.GetComponentsInChildren<Transform>();
            tempTitlePanel.name = "TitlePanel_" + tsData.GetPrimaryKey();
            bool isExpanded = false;

            if(transactionGameObjectDict.ContainsKey(tsData.GetPrimaryKey()))
            {
                transactionGameObjects = transactionGameObjectDict[tsData.GetPrimaryKey()];
                isExpanded = transactionGameObjects.IsExpanded();
                transactionGameObjectDict.Remove(tsData.GetPrimaryKey());
            }

            foreach (Transform t in ts)
            {
                switch (t.gameObject.name)
                {
                    case "MainTitle":
                        t.gameObject.GetComponent<Text>().text = tsData.GetPrimaryKey();
                        break;
                    case "ExpandCollapseButton":
                        if (isExpanded)
                        {
                            t.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text = "v";
                        }
                        else
                        {
                            t.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text = ">";
                        }
                        break;
                }
            }
            tempTitlePanel.transform.SetParent(mainPanel.transform, false);
            tempTitlePanel.SetActive(true);
            titlePanel.SetActive(false);

            transactionGameObjects = new TransactionSummaryGameObjectList(tsData.GetPrimaryKey(), tempTitlePanel);
            transactionGameObjects.SetExpandedFlag(isExpanded);
            transactionGameObjectDict.Add(tsData.GetPrimaryKey(), transactionGameObjects);
        }
    }

    public override void OnDefaultButtonClick()
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, TransactionSummaryGameObjectList> ExpandCollapseBasePanel(bool shouldExpandPanel, TransactionSummaryData tsData, 
        Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {

        TransactionSummaryGameObjectList transactionGameObjects = transactionGameObjectDict[tsData.GetPrimaryKey()];
        transactionGameObjects.GetTransactionSummaryGameObjects().Clear();
        GameObject parentGameObject = transactionGameObjects.GetParentGameObject();
        GameObject expandCollapseButtonObject = parentGameObject.transform.GetComponentsInChildren<Button>()[1].gameObject;

        if (!shouldExpandPanel)
        {
            expandCollapseButtonObject.GetComponentInChildren<Text>().text = ">";
            transactionGameObjects.SetExpandedFlag(false);
        }
        else
        {
            expandCollapseButtonObject.GetComponentInChildren<Text>().text = "v";
            transactionGameObjects.SetExpandedFlag(true);
            
            foreach (TransactionSummaryFirstSubData tsFirstSubData in tsData.GetTransactionSummaryFirstSubDataList())
            {
                GameObject tempFirstSubPanel = (GameObject)Instantiate(firstSubPanel, transform.position, transform.rotation);
                Transform[] ts = tempFirstSubPanel.transform.GetComponentsInChildren<Transform>();
                tempFirstSubPanel.name = "FirstSubPanel_" + tsData.GetPrimaryKey() + "_" + tsFirstSubData.GetPrimaryKey();

                foreach (Transform t in ts)
                {
                    switch (t.gameObject.name)
                    {
                        case "FirstSubPanelMainText":
                            t.gameObject.GetComponent<Text>().text = tsFirstSubData.GetPrimaryKey();
                            break;
                        case "FirstSubPanelSubText":
                            t.gameObject.GetComponent<Text>().text = "";
                            break;
                        case "FirstSubPanelPrice":
                            t.gameObject.GetComponent<Text>().text = "$" + tsFirstSubData.GetTotalPrice();
                            break;
                    }
                }

                tempFirstSubPanel.SetActive(true);
                tempFirstSubPanel.transform.SetParent(parentGameObject.transform, false);
                transactionGameObjects.AddTransactionSummaryGameObject(tempFirstSubPanel);
            }
        }
        return transactionGameObjectDict;
    }

    public Dictionary<string, TransactionSummaryGameObjectList> ExpandCollapseFirstPanel(bool shouldExpandPanel, TransactionSummaryFirstSubData selectedTransSubData, string parentKey, 
        Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {
        if (!shouldExpandPanel)
        {
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
        }
        else 
        {
            GameObject parentGameObject = transactionGameObjectDict[parentKey].GetParentGameObject();
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
            GameObject selectedSubPanel = parentGameObject.transform.Find("FirstSubPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey()).gameObject;
            TransactionSummaryGameObjectList transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), selectedSubPanel);
            transactionGameObjects.SetExpandedFlag(true);
            transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

            foreach (TransactionSummarySecondSubData tsSecondSubData in selectedTransSubData.GetTransactionSummarySecondSubDataList())
            {
                GameObject tempSecondSubPanel = (GameObject)Instantiate(secondSubPanel, transform.position, transform.rotation);
                Transform[] ts = tempSecondSubPanel.transform.GetComponentsInChildren<Transform>();
                tempSecondSubPanel.name = "SecondSubPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey() + "_" + tsSecondSubData.GetPrimaryKey();

                foreach (Transform t in ts)
                {
                    switch (t.gameObject.name)
                    {
                        case "SecondSubPanelMainText":
                            t.gameObject.GetComponent<Text>().text = tsSecondSubData.GetPrimaryKey();
                            break;
                        case "SecondSubPanelSubText":
                            t.gameObject.GetComponent<Text>().text = "";
                            break;
                        case "SecondSubPanelPrice":
                            t.gameObject.GetComponent<Text>().text = "$" + tsSecondSubData.GetTotalPrice();
                            break;
                    }
                }
                            
                tempSecondSubPanel.SetActive(true);
                tempSecondSubPanel.transform.SetParent(selectedSubPanel.transform, false);
                transactionGameObjects.AddTransactionSummaryGameObject(tempSecondSubPanel);
            }
        }

        return transactionGameObjectDict;
    }

    public Dictionary<string, TransactionSummaryGameObjectList> ExpandCollapseSecondPanel(bool shouldExpandPanel, TransactionSummarySecondSubData selectedTransSubData, string parentKey,
        Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {
        if (!shouldExpandPanel)
        {
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
        }
        else
        {
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
            GameObject parentGameObject = transactionGameObjectDict[parentKey].GetParentGameObject();

            GameObject selectedSubPanel = parentGameObject.transform.Find("SecondSubPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey()).gameObject;

            TransactionSummaryGameObjectList transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), selectedSubPanel);
            transactionGameObjects.SetExpandedFlag(true);
            transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

            foreach (TransactionSummarySubDetailsData tsSubDetailsData in selectedTransSubData.GetTransactionSummarySubDetailsData())
            {
                GameObject tempThirdSubPanel = (GameObject)Instantiate(thirdSubPanel, transform.position, transform.rotation);
                Transform[] ts = tempThirdSubPanel.transform.GetComponentsInChildren<Transform>();
                tempThirdSubPanel.name = "ThirdSubPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey() + "_" + tsSubDetailsData.GetPrimaryKey();


                string firstSubPanelSubText = "";
                bool shouldSubTextBeVisible = false;
                if (isOrderByDate)
                {
                    firstSubPanelSubText = tsSubDetailsData.GetPrimaryKey().Substring(0, 1);
                    shouldSubTextBeVisible = true;
                }

                foreach (Transform t in ts)
                {
                    switch (t.gameObject.name)
                    {
                        case "ThirdSubPanelMainText":
                            t.gameObject.GetComponent<Text>().text = tsSubDetailsData.GetPrimaryKey();
                            break;
                        case "ThirdSubPanelSubTextImage":
                            t.gameObject.GetComponent<Button>().GetComponentInChildren<Text>().text = firstSubPanelSubText;
                            t.gameObject.GetComponent<Button>().gameObject.SetActive(shouldSubTextBeVisible);
                            break;
                        case "ThirdSubPanelPrice":
                            t.gameObject.GetComponent<Text>().text = "$" + tsSubDetailsData.GetTotalPrice();
                            break;
                    }
                }

                tempThirdSubPanel.SetActive(true);
                tempThirdSubPanel.transform.SetParent(selectedSubPanel.transform, false);
                transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempThirdSubPanel);
            }
        }
        return transactionGameObjectDict;
    }

    public Dictionary<string, TransactionSummaryGameObjectList> ExpandCollapseThirdPanel(
        bool shouldExpandPanel, TransactionSummarySubDetailsData selectedTransSubData, string parentKey,
        Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict)
    {
        if (!shouldExpandPanel)
        {
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
        }
        else
        {
            transactionGameObjectDict.Remove(parentKey + "_" + selectedTransSubData.GetPrimaryKey());

            GameObject parentGameObject = transactionGameObjectDict[parentKey].GetParentGameObject();

            GameObject selectedSubPanel = parentGameObject.transform.Find("ThirdSubPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey()).gameObject;
            TransactionSummaryGameObjectList transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), selectedSubPanel);
            transactionGameObjects.SetExpandedFlag(true);
            transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

            foreach (TransactionSummaryDetailsData tsDetailsData in selectedTransSubData.getTransactionSummaryDetailsData())
            {
                GameObject tempTransactionsPanel = (GameObject)Instantiate(transactionsPanel, transform.position, transform.rotation);
                Transform[] ts = tempTransactionsPanel.transform.GetComponentsInChildren<Transform>();
                tempTransactionsPanel.name = "TransactionsPanel_" + parentKey + "_" + selectedTransSubData.GetPrimaryKey() + "_" + tsDetailsData.GetPrimaryKey();

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
                tempTransactionsPanel.transform.SetParent(selectedSubPanel.transform, false);
                transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempTransactionsPanel);
            }
        }
        return transactionGameObjectDict;
    }
}
