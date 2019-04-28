using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionDetailsList : ContentList
{
    public TransactionDetailsRow transactionDetailsRowPrefab;
    public TransactionDetailsPanel transactionDetailsPanel;

    public TransactionDetailsRow AddRow()
    {
        TransactionDetailsRow row = Instantiate(transactionDetailsRowPrefab, contentPanel.transform);
        row.transform.SetAsLastSibling();
        contentList.Add(row);
        ResetAllRows();

        return row;
    }
}
