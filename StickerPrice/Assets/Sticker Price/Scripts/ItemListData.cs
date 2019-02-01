using System.Collections.Generic;
using System;

[Serializable]
public class ItemListData
{
    public class ItemRowDataListContainer
    {
        public List<ItemRowData> itemRowDataList = new List<ItemRowData>();
    }

    public float priceTotal = 0f;
    public float priceSubtotal = 0f;
    public float taxTotal = 0f;
    public float discount = 0f;
    public int itemTotal = 0;
    public ItemRowDataListContainer itemRowDataListContainer = new ItemRowDataListContainer();
}