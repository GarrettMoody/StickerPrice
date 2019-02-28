using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionByDateList : ContentList
{
    public TransactionByDateRow transactionByDateRowPrefab;
    public TrackerSummary trackerSummaryPanel;

    public TransactionByDateRow AddRow()
    {
        TransactionByDateRow row = Instantiate(transactionByDateRowPrefab, contentPanel.transform);
        row.transform.SetAsLastSibling();
        contentList.Add(row);
        ResetAllRows();

        return row;
    }
}
