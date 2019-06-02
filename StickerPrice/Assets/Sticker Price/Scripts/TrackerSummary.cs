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
    public TransactionDetailsPanel transactionDetailsPanel;
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

    public void LoadTransactionListFromFile()
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

        if (transactionList != null && transactionList.Count > 0)
        {
            TransactionByDateRow row = transactionByDateList.AddRow();
            row.InitiateTransactionRows(transactionList, dateToggle.isOn, transactionGameObjectDict);

            foreach (TransactionSummaryData tsData in transactionList)
            {
                GameObject tempCurrentRow = transactionGameObjectDict[tsData.GetPrimaryKey()].GetParentGameObject();
                tempCurrentRow.transform.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => onClickRow(false, tsData.GetPrimaryKey()));
            }
        }
    }

    public void onClickRow(bool isAlreadyExpanded, string selectedPanel)
    {

        transactionByDateList.RemoveAllRows();

        if (transactionSummaryDataList != null && transactionSummaryDataList.Count > 0)
        {
            TransactionByDateRow row = transactionByDateList.AddRow();
            row.InitiateTransactionRows(transactionSummaryDataList, dateToggle.isOn, transactionGameObjectDict);

            foreach (TransactionSummaryData tsData in transactionSummaryDataList)
            {
                string currentMainPanelkey = tsData.GetPrimaryKey();
                TransactionSummaryGameObjectList transactionGameObjects = null;
                bool tempBaseRowExpandedFlag = false;

                if (selectedPanel.Equals(currentMainPanelkey))
                {
                    tempBaseRowExpandedFlag = !isAlreadyExpanded;
                    transactionGameObjectDict = row.ExpandCollapseBasePanel(tempBaseRowExpandedFlag, tsData, transactionGameObjectDict);
                    transactionGameObjects = transactionGameObjectDict[currentMainPanelkey];
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

                GameObject tempCurrentRow = transactionGameObjects.GetParentGameObject();
                tempCurrentRow.transform.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => onClickRow(tempBaseRowExpandedFlag, tsData.GetPrimaryKey()));

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
                                        currentThirdPanel.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => openTransactionDetailsView(currentFinalSubPanelkey, tsSubDetailsData));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void openTransactionDetailsView(string selectedPanel, TransactionSummarySubDetailsData selectedMainPanelSubDetailsData)
    {
        Debug.Log("openTransactionDetailsView.selectedMainPanel>>>" + selectedPanel);

        this.gameObject.SetActive(false);
        transactionDetailsPanel.selectedMainPanelKey = selectedPanel;
        transactionDetailsPanel.selectedMainPanelSubDetailsData = selectedMainPanelSubDetailsData;
        transactionDetailsPanel.isOwnerSelected = ownerToggle.isOn;
        transactionDetailsPanel.LoadTransactionListFromFile();
        transactionDetailsPanel.gameObject.SetActive(true);
    }
}