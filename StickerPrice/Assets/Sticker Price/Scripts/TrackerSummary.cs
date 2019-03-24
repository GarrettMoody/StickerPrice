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
    public TransactionByDateRow transactionByDateRowPrefab;
    List<TransactionSummaryData> transactionSummaryDataList;

    public GameObject gridThatStoresTheItems;
    public Text itemPrefab;
    int i = 0;
    bool flag = false;

    public void Awake()
    {
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

        foreach (TransactionSummaryData transaction in transactionList)
        {
            TransactionByDateRow row = transactionByDateList.AddRow();
            row.InitiateTransactionByDateRow(transaction, dateToggle.isOn);
        }
    }

    //public void OpenFavoriteSticker(Sticker sticker)
    //{
    //    this.gameObject.SetActive(false);
    //    stickerDetailMenu.OpenMenu(sticker);
    //}

    //public void OpenEditFavoriteSticker(Sticker sticker)
    //{
    //    this.gameObject.SetActive(false);
    //    editStickerPanel.OpenEditStickerPanel(sticker);
    //}
}
