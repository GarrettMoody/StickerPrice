using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using System.Globalization;

public class ScanPanel : MonoBehaviour
{

    public RawImage scanDisplay;
    public Text currentDateTime;
    public Button ToggleCameraButton;
    public CheckoutPanel checkoutPanel;
    public TextMeshProUGUI transactionNumber;
    public ItemList itemList;
    public GameObject popupPanel;
    public AddItemPopup addItemPopup;

    private WebCamTexture camTexture;
    private bool scanReady = false; //Is the camera accepting QR code input?
    private float scanTimer; //Time remaining for scanner to be turned on
    private Transaction transaction;

    [Tooltip("The amount of time the scanner will read after clicking the scan button.")]
    public const float SCAN_TIMER = .25f;
    public PopupMessage scanError;


    // Use this for initialization
    void Start()
    {
        camTexture = new WebCamTexture(WebCamTexture.devices[0].name, 480, 640, 60);
        scanDisplay.texture = camTexture;
        scanDisplay.material.mainTexture = camTexture;
        camTexture.Play();
    }

    void Update()
    {
        currentDateTime.text = System.DateTime.Now.ToString("dd MMM yyyy hh:mm tt");

        if (scanReady)
        {
            scanTimer -= Time.deltaTime;

            IBarcodeReader barcodeReader = new BarcodeReader();
            //decode the current frame
            if (camTexture.isPlaying)
            {
                Result result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

                if (result != null)
                {
                    QRCodeScanned(result);
                }
            }

            if (scanTimer <= 0f)
            {
                scanReady = false;
                scanError.DisplayPopup(4);
            }
        }
    }

    private void OnDisable()
    {
        camTexture.Pause();
    }

    private void OnEnable()
    {
        //If camera isn't playing, start camera
        if(camTexture != null)
        {
            camTexture.Play();
        }

        //Initialize the transaction if one is not already initialized.
        TransactionData transactionData = new TransactionData();
        if(transaction == null || transaction.transactionID == 0f)
        {
            StartNewTransaction();
        }
    }

    public void ScanButtonOnClickListener()
    {
        scanReady = true;
        scanTimer = SCAN_TIMER;
    }

    void QRCodeScanned(Result result)
    {
        scanReady = false;
        bool addItem = true; //do we still need to add the item to the list?
        foreach (ItemRow row in itemList.GetRows())
        {
            if (row.GetScanString() == result.Text)
            { //Does item already exist in list
                row.SetQuantity(row.GetQuantity() + 1); //item already exists, add quantity
                addItem = false;
            }
        }

        //Check if we still need to add the item to the list or if we already added quantity to an existing item
        if (addItem)
        { 
            String[] resultString = new string[4];
            resultString = result.Text.Split('|');

            float itemPrice = float.Parse(resultString[1], NumberStyles.Currency); //Get the value of the float, ignoring the currency format

            ItemRow newItem = itemList.AddItem();
            newItem.SetScanString(result.Text);
            newItem.SetItemDescription(resultString[2]);
            newItem.SetProductOwner(resultString[3]);
            newItem.SetItemPrice(itemPrice);
            newItem.SetItemOriginalPrice(itemPrice);
        } 

        //update the transactions itemlist
        transaction.SetItemListData(itemList.itemListData);

        //Update checkout button

    }

    public void ToggleCameraButtonOnClickListener()
    {
        if (camTexture.isPlaying)
        {
            camTexture.Stop();
            ToggleCameraButton.image.color = new Color(1, 1, 1, .8f);
        }
        else
        {
            camTexture.Play();
            ToggleCameraButton.image.color = new Color(1, 1, 1, 0);
        }
    }

    public void CheckoutButtonOnClickListener() {
        this.gameObject.SetActive(false);
        checkoutPanel.gameObject.SetActive(true);

        //Rebuild the list of rows in the checkout panel
        checkoutPanel.itemList.RemoveAllRows();
        foreach (ItemRow row in itemList.GetRows()) {
            ItemRow newRow = checkoutPanel.itemList.AddItem(row);
            newRow.UpdatePriceText();
        }

        //Send all data to the checkout panel
        checkoutPanel.itemList.SetItemTotal(this.itemList.GetItemTotal());
        checkoutPanel.itemList.SetPriceSubtotal(this.itemList.GetPriceSubtotal());
        checkoutPanel.itemList.SetTaxTotal(this.itemList.GetTaxTotal());
        checkoutPanel.SetTransactionNumber(GetTransactionNumber());
        checkoutPanel.SetTransaction(transaction);
    }

    public void AddItemOnClickListener()
    {
        popupPanel.gameObject.SetActive(true);
        addItemPopup.gameObject.SetActive(true); 
    }

    public void AddItem(Sticker sticker)
    {
        ItemRow newItem = itemList.AddItem();
        //newItem.SetScanString(sticker.);
        newItem.SetItemDescription(sticker.itemDescription);
        newItem.SetProductOwner(sticker.owner);
        newItem.SetItemPrice(float.Parse(sticker.price));
        newItem.SetItemOriginalPrice(float.Parse(sticker.price));

        transaction.SetItemListData(itemList.itemListData);
    }

    public string GetTransactionNumber()
    {
        return transactionNumber.text;
    }

    public void SetTransactionNumber(string number)
    {
        transactionNumber.text = number;
    }

    public void StartNewTransaction()
    {
        TransactionData transactionData = new TransactionData();
        transaction = new Transaction(transactionData.NextTransactionID());
        SetTransactionNumber(transaction.transactionID.ToString());
        itemList.RemoveAllRows();
    }
}