using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackerSummary : MonoBehaviour
{
    public ToggleGroup sortType;
    public Toggle dateToggle;
    public Toggle ownerToggle;
    public TransactionByDateList transactionByDateList;
    List<TransactionSummaryData> transactionSummaryDataList;
    Dictionary<string, TransactionSummaryGameObjectList> transactionGameObjectDict;

    public GameObject contentPanel;

    public TransactionByDateRow transactionByDateRowPrefab;


    public void Awake()
    {
        transactionGameObjectDict = new Dictionary<string, TransactionSummaryGameObjectList>();
        LoadTransactionListFromFile();
    }

    //public void OpenSavedFavoritesPanel()
    //{
    //    this.gameObject.SetActive(true);
    //    LoadFavoriteStickerListFromFile();
    //}

    private void LoadTransactionListFromFile()
    {

        TransactionData transactionData = new TransactionData();

        if (ownerToggle.isOn)
        {
            transactionSummaryDataList = transactionData.GetTransactionsSortedByOwner();
            LoadTransactionList(transactionSummaryDataList);
        }
        else
        {
            transactionSummaryDataList = transactionData.GetTransactionsSortedByDate();
            LoadTransactionList(transactionSummaryDataList);
        }
    }

    private void LoadTransactionList(List<TransactionSummaryData> transactionList)
    {
        transactionByDateList.RemoveAllRows();

        foreach (TransactionSummaryData tsData in transactionList)
        {
            TransactionByDateRow row = transactionByDateList.AddRow();
            row.InitiateTransactionByDateRow(tsData, dateToggle.isOn);
            row.expandCollapseButton.onClick.AddListener(() => onClickRow(false, tsData.GetPrimaryKey()));
        }
    }

    public void onClickRow(bool isAlreadyExpanded, string selectedPanel)
    {
        transactionByDateList.RemoveAllRows();

        foreach (TransactionSummaryData tsData in transactionSummaryDataList)
        {
            TransactionByDateRow row = transactionByDateList.AddRow();
            row.InitiateTransactionByDateRow(tsData, dateToggle.isOn);
            string currentMainPanelkey = tsData.GetPrimaryKey();
            TransactionSummaryGameObjectList transactionGameObjects = null;
            bool tempBaseRowExpandedFlag = false;

            if (selectedPanel.Equals(currentMainPanelkey))
            {
                tempBaseRowExpandedFlag = !isAlreadyExpanded;
                transactionGameObjectDict = row.ExpandCollapseBasePanel(tempBaseRowExpandedFlag, tsData, transactionGameObjectDict);
            }
            else
            {
                if (transactionGameObjectDict.ContainsKey(currentMainPanelkey))
                {
                    transactionGameObjects = transactionGameObjectDict[currentMainPanelkey];
                    tempBaseRowExpandedFlag = transactionGameObjects.IsExpanded();
                }

                if (tempBaseRowExpandedFlag)
                {
                    transactionGameObjectDict = row.ExpandCollapseBasePanel(tempBaseRowExpandedFlag, tsData, transactionGameObjectDict);
                }
            }

            row.expandCollapseButton.onClick.AddListener(() => onClickRow(tempBaseRowExpandedFlag, currentMainPanelkey));

            if (tempBaseRowExpandedFlag)
            {
                foreach (TransactionSummaryFirstSubData tsFirstSubData in tsData.GetTransactionSummaryFirstSubDataList())
                {
                    string currentFirstSubPanelkey = currentMainPanelkey + "_" + tsFirstSubData.GetPrimaryKey();
                    bool tempFirstRowExpandedFlag = false;

                    if ((currentFirstSubPanelkey).Equals(selectedPanel))
                    {
                        tempFirstRowExpandedFlag = !isAlreadyExpanded;
                        transactionGameObjectDict = row.ExpandCollapseFirstPanel(tempFirstRowExpandedFlag, tsFirstSubData, currentMainPanelkey, transactionGameObjectDict);
                    }
                    else
                    {
                        if (transactionGameObjectDict.ContainsKey(currentFirstSubPanelkey))
                        {
                            transactionGameObjects = transactionGameObjectDict[currentFirstSubPanelkey];
                            tempFirstRowExpandedFlag = transactionGameObjects.IsExpanded();

                            if (tempFirstRowExpandedFlag)
                            {
                                transactionGameObjectDict = row.ExpandCollapseFirstPanel(tempFirstRowExpandedFlag, tsFirstSubData, currentMainPanelkey, transactionGameObjectDict);
                            }
                        }
                    }

                    transactionGameObjects = transactionGameObjectDict[currentMainPanelkey];
                    GameObject currentFirstPanel = transactionGameObjects.GetParentGameObject().transform.Find("FirstSubPanel_" + currentFirstSubPanelkey).gameObject;
                    currentFirstPanel.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => onClickRow(tempFirstRowExpandedFlag, currentFirstSubPanelkey));

                    if (tempFirstRowExpandedFlag)
                    {
                        foreach (TransactionSummarySecondSubData tsSecondSubData in tsFirstSubData.GetTransactionSummarySecondSubDataList())
                        {
                            string currentSecondSubPanelkey = currentFirstSubPanelkey + "_" + tsSecondSubData.GetPrimaryKey();
                            bool curSecondPanelExpandedFlag = false;

                            if ((currentSecondSubPanelkey).Equals(selectedPanel))
                            {
                                curSecondPanelExpandedFlag = !isAlreadyExpanded;
                                transactionGameObjectDict = row.ExpandCollapseSecondPanel(curSecondPanelExpandedFlag, tsSecondSubData, currentFirstSubPanelkey, transactionGameObjectDict);
                            }
                            else
                            {
                                if (transactionGameObjectDict.ContainsKey(currentSecondSubPanelkey))
                                {
                                    transactionGameObjects = transactionGameObjectDict[currentSecondSubPanelkey];
                                    curSecondPanelExpandedFlag = transactionGameObjects.IsExpanded();

                                    if (curSecondPanelExpandedFlag)
                                    {
                                        transactionGameObjectDict = row.ExpandCollapseSecondPanel(curSecondPanelExpandedFlag, tsSecondSubData, currentFirstSubPanelkey, transactionGameObjectDict);
                                    }
                                }
                            }

                            transactionGameObjects = transactionGameObjectDict[currentFirstSubPanelkey];
                            GameObject currentSecondPanel = transactionGameObjects.GetParentGameObject().transform.Find("SecondSubPanel_" + currentSecondSubPanelkey).gameObject;
                            currentSecondPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => onClickRow(curSecondPanelExpandedFlag, currentSecondSubPanelkey));

                            if (curSecondPanelExpandedFlag)
                            {
                                foreach (TransactionSummarySubDetailsData tsSubDetailsData in tsSecondSubData.GetTransactionSummarySubDetailsData())
                                {
                                    string currentFinalSubPanelkey = currentSecondSubPanelkey + "_" + tsSubDetailsData.GetPrimaryKey();
                                    bool curFinalPanelExpandedFlag = false;

                                    if ((currentFinalSubPanelkey).Equals(selectedPanel))
                                    {
                                        curFinalPanelExpandedFlag = !isAlreadyExpanded;
                                        transactionGameObjectDict = row.ExpandCollapseThirdPanel(curFinalPanelExpandedFlag, tsSubDetailsData, currentSecondSubPanelkey, transactionGameObjectDict);
                                    }
                                    else
                                    {
                                        if (transactionGameObjectDict.ContainsKey(currentFinalSubPanelkey))
                                        {
                                            transactionGameObjects = transactionGameObjectDict[currentFinalSubPanelkey];
                                            curFinalPanelExpandedFlag = transactionGameObjects.IsExpanded();

                                            if (curFinalPanelExpandedFlag)
                                            {
                                                transactionGameObjectDict = row.ExpandCollapseThirdPanel(curFinalPanelExpandedFlag, tsSubDetailsData, currentSecondSubPanelkey, transactionGameObjectDict);
                                            }
                                        }
                                    }

                                    transactionGameObjects = transactionGameObjectDict[currentSecondSubPanelkey];
                                    GameObject currentThirdPanel = transactionGameObjects.GetParentGameObject().transform.Find("ThirdSubPanel_" + currentFinalSubPanelkey).gameObject;
                                    currentThirdPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => onClickRow(curFinalPanelExpandedFlag, currentFinalSubPanelkey));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
