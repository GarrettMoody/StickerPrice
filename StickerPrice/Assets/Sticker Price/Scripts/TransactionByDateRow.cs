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

    public TransactionSummaryData transaction;
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
        this.transaction = transaction;
        mainTitle.text = transaction.primaryKey;
        this.isOrderByDate = isOrderByDate;
        firstSubPanel.SetActive(false);
        secondSubPanel.SetActive(false);
        thirdSubPanel.SetActive(false);
        transactionsPanel.SetActive(false);
        expandCollapseButton.GetComponentInChildren<Text>().text = ">";
    }

    public override void OnDefaultButtonClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnExpandButtonClick()
    {
        bool alreadyVisible = false;
        TransactionSummaryGameObjectList transactionGameObjects = null;

        if (transactionGameObjectDict.ContainsKey(transaction.GetPrimaryKey()))
        {
            transactionGameObjects = transactionGameObjectDict[transaction.GetPrimaryKey()];
            alreadyVisible = transactionGameObjects.visible;
        }

        if (alreadyVisible)
        {
            expandCollapseButton.GetComponentInChildren<Text>().text = ">";
            transactionGameObjects.visible = false;

            List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

            foreach (GameObject gameObj in transactionSummaryGameObjectList)
            {
                gameObj.SetActive(false);
            }
        }
        else
        {
            expandCollapseButton.GetComponentInChildren<Text>().text = "v";

            if (transactionGameObjects != null && transactionGameObjects.GetTransactionSummaryGameObjects().Count > 0)
            {
                transactionGameObjects.visible = true;

                List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

                foreach (GameObject gameObj in transactionSummaryGameObjectList)
                {
                    gameObj.SetActive(true);
                }
            }
            else
            {
                transactionGameObjects = new TransactionSummaryGameObjectList(transaction.GetPrimaryKey());
                transactionGameObjects.visible = true;
                transactionGameObjectDict.Add(transaction.GetPrimaryKey(), transactionGameObjects);

                foreach (TransactionSummaryFirstSubData tsFirstSubData in transaction.GetTransactionSummaryFirstSubDataList())
                {
                    GameObject tempFirstSubPanel = (GameObject)Instantiate(firstSubPanel, transform.position, transform.rotation);
                    Transform[] ts = tempFirstSubPanel.transform.GetComponentsInChildren<Transform>();
                    
                    foreach (Transform t in ts)
                    {
                        switch (t.gameObject.name)
                        {
                            case "FirstSubPanelMainText":
                                t.gameObject.GetComponent<Text>().text = tsFirstSubData.GetPrimaryKey() + "\n $" + tsFirstSubData.GetTotalPrice();
                                break;
                            case "FirstSubPanelSubText":
                                t.gameObject.GetComponent<Text>().text = "";
                                break;
                        }
                    }

                    tempFirstSubPanel.SetActive(true);
                    tempFirstSubPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => OnFirstSubExpandButtonClick(tsFirstSubData, tempFirstSubPanel, transaction.GetPrimaryKey()));
                    tempFirstSubPanel.transform.SetParent(titlePanel.transform, false);
                    transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempFirstSubPanel);
                }
            }
        }
    }

    public void OnFirstSubExpandButtonClick(TransactionSummaryFirstSubData selectedTransSubData, GameObject selectedSubPanel, string parentKey)
    {
        bool alreadyVisible = false;
        TransactionSummaryGameObjectList transactionGameObjects = null;

        if (transactionGameObjectDict.ContainsKey(parentKey + "_" + selectedTransSubData.GetPrimaryKey()))
        {
            transactionGameObjects = transactionGameObjectDict[parentKey + "_" + selectedTransSubData.GetPrimaryKey()];
            alreadyVisible = transactionGameObjects.visible;
        }


        if (alreadyVisible)
        {
            transactionGameObjects.visible = false;

            List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

            foreach (GameObject gameObj in transactionSummaryGameObjectList)
            {
                gameObj.SetActive(false);
            }
        }
        else
        {
            if (transactionGameObjects != null && transactionGameObjects.GetTransactionSummaryGameObjects().Count > 0)
            {
                transactionGameObjects.visible = true;

                List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

                foreach (GameObject gameObj in transactionSummaryGameObjectList)
                {
                    gameObj.SetActive(true);
                }
            }
            else 
            {
                transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
                transactionGameObjects.visible = true;
                transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

                foreach (TransactionSummarySecondSubData tsSecondSubData in selectedTransSubData.GetTransactionSummarySecondSubDataList())
                {
                    GameObject tempSecondSubPanel = (GameObject)Instantiate(secondSubPanel, transform.position, transform.rotation);
                    Transform[] ts = tempSecondSubPanel.transform.GetComponentsInChildren<Transform>();

                    foreach (Transform t in ts)
                    {
                        switch (t.gameObject.name)
                        {
                            case "SecondSubPanelMainText":
                                t.gameObject.GetComponent<Text>().text = tsSecondSubData.GetPrimaryKey() + "\n$" + tsSecondSubData.GetTotalPrice();
                                break;
                            case "SecondSubPanelSubText":
                                t.gameObject.GetComponent<Text>().text = "";
                                break;
                        }
                    }

                    tempSecondSubPanel.SetActive(true);
                    tempSecondSubPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => OnSecondSubExpandButtonClick(tsSecondSubData, tempSecondSubPanel, parentKey + "_" + selectedTransSubData.GetPrimaryKey()));
                    tempSecondSubPanel.transform.SetParent(selectedSubPanel.transform, false);
                    transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempSecondSubPanel);
                }
            }
        }
    }

    public void OnSecondSubExpandButtonClick(TransactionSummarySecondSubData selectedTransSubData, GameObject selectedSubPanel, string parentKey)
    {
        bool alreadyVisible = false;
        TransactionSummaryGameObjectList transactionGameObjects = null;

        if (transactionGameObjectDict.ContainsKey(parentKey + "_" + selectedTransSubData.GetPrimaryKey()))
        {
            transactionGameObjects = transactionGameObjectDict[parentKey + "_" + selectedTransSubData.GetPrimaryKey()];
            alreadyVisible = transactionGameObjects.visible;
        }


        if (alreadyVisible)
        {
            transactionGameObjects.visible = false;
        }
        else
        {
            if (transactionGameObjects != null && transactionGameObjects.GetTransactionSummaryGameObjects().Count > 0)
            {
                transactionGameObjects.visible = true;
            }
            else
            {
                transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
                transactionGameObjects.visible = true;
                transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

                foreach (TransactionSummarySubDetailsData tsSubDetailsData in selectedTransSubData.GetTransactionSummarySubDetailsData())
                {
                    GameObject tempThirdSubPanel = (GameObject)Instantiate(thirdSubPanel, transform.position, transform.rotation);
                    Transform[] ts = tempThirdSubPanel.transform.GetComponentsInChildren<Transform>();


                    string firstSubPanelSubText = "";
                    if (isOrderByDate)
                    {
                        firstSubPanelSubText = tsSubDetailsData.GetPrimaryKey().Substring(0, 1);
                    }

                    foreach (Transform t in ts)
                    {
                        switch (t.gameObject.name)
                        {
                            case "ThirdSubPanelMainText":
                                t.gameObject.GetComponent<Text>().text = tsSubDetailsData.GetPrimaryKey() + " \n $" + tsSubDetailsData.GetTotalPrice();
                                break;
                            case "ThirdSubPanelSubText":
                                t.gameObject.GetComponent<Text>().text = firstSubPanelSubText;
                                break;
                        }
                    }

                    tempThirdSubPanel.SetActive(true);
                    tempThirdSubPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => OnSubpanelExpandFinalButtonClick(tsSubDetailsData, tempThirdSubPanel, parentKey + "_" + selectedTransSubData.GetPrimaryKey()));
                    tempThirdSubPanel.transform.SetParent(selectedSubPanel.transform, false);
                    transactionGameObjects.GetTransactionSummaryGameObjects().Add(tempThirdSubPanel);
                }
            }
        }
    }

    public void OnSubpanelExpandFinalButtonClick(TransactionSummarySubDetailsData selectedTransSubData, GameObject selectedSubPanel, string parentKey)
    {
        bool alreadyVisible = false;
        TransactionSummaryGameObjectList transactionGameObjects = null;

        if (transactionGameObjectDict.ContainsKey(parentKey + "_" + selectedTransSubData.GetPrimaryKey()))
        {
            TransactionSummaryGameObjectList tmpObjs = transactionGameObjectDict[parentKey + "_" + selectedTransSubData.GetPrimaryKey()];

            transactionGameObjects = transactionGameObjectDict[parentKey + "_" + selectedTransSubData.GetPrimaryKey()];
            alreadyVisible = transactionGameObjects.visible;
        }

        if (alreadyVisible)
        {
            transactionGameObjects.visible = false;

            List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

            foreach (GameObject gameObj in transactionSummaryGameObjectList)
            {
                gameObj.SetActive(false);
            }
        }
        else
        {
            if (transactionGameObjects != null && transactionGameObjects.GetTransactionSummaryGameObjects().Count > 0)
            {
                transactionGameObjects.visible = true;

                List<GameObject> transactionSummaryGameObjectList = transactionGameObjects.GetTransactionSummaryGameObjects();

                foreach (GameObject gameObj in transactionSummaryGameObjectList)
                {
                    gameObj.SetActive(true);
                }
            }
            else
            {
                transactionGameObjects = new TransactionSummaryGameObjectList(parentKey + "_" + selectedTransSubData.GetPrimaryKey());
                transactionGameObjects.visible = true;
                transactionGameObjectDict.Add(parentKey + "_" + selectedTransSubData.GetPrimaryKey(), transactionGameObjects);

                foreach (TransactionSummaryDetailsData tsDetailsData in selectedTransSubData.getTransactionSummaryDetailsData())
                {
                    GameObject tempTransactionsPanel = (GameObject)Instantiate(transactionsPanel, transform.position, transform.rotation);
                    Transform[] ts = tempTransactionsPanel.transform.GetComponentsInChildren<Transform>();


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
        }
    }
}
