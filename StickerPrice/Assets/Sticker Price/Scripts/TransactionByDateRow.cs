using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TransactionByDateRow : ContentRow, IPointerClickHandler
{

    public Button expandButton;
    public Text transactionDate;
    public GameObject titlePanel;

    public override void OnDefaultButtonClick()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
