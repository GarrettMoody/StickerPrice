using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class ItemRowData
{
    public string itemDescription = "New Item";
    public float itemOriginalPrice = 10;
    public float itemPrice = 10;
    public int quantity = 1;
    public string productOwner;
    public string scanString;
}
