using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdjustPanel : MonoBehaviour
{
	//UI Components
	public Text itemDescription;
	public InputField inputField;
	public ToggleGroup modeButtonPanel;
	public Text originalPriceText;
	public Text adjustedPriceText;
    public Sprite selectedImage;
    public Sprite unselectedImage;

	private ItemRow itemRow;
    private GameObject numberButtonsPanel;

	//Price Variables
	private float adjustedPrice;
	private float originalPrice;
	private float percentDiscount;
	private float dollarDiscount;
	private float inputFieldValue;

	private string priceMode = DOLLARS_OFF;
	//priceMode Constants
	private const string DOLLARS_OFF = "DollarsOff";
	private const string PERCENT_OFF = "PercentOff";

    //Constants
    readonly Color32 THEME_GREEN = new Color32(0x5C, 0xAB, 0x40, 0xFF);
    readonly Color32 RED = new Color32(0xE2, 0x23, 0x1A, 0xFF);
    readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    readonly Color32 DARK_GREY = new Color32(0x52, 0x53, 0x49, 0xFF);

	public void OpenAdjustPanel (ItemRow row)
	{
		itemRow = row;
		InitializeVariables ();
		this.transform.parent.gameObject.SetActive (true);
		this.gameObject.SetActive (true);
	}

    private void InitializeVariables ()
	{
		SetItemDescriptionText (itemRow.GetItemDescription ());
		SetOriginalPrice (itemRow.GetItemOriginalPrice ());
		SetInputFieldValue (0);
        OnPriceModeChange();
		adjustedPrice = originalPrice;
	}

    public void OnNumberButtonPress (int number)
	{
		SetInputFieldValue (float.Parse (inputFieldValue.ToString () + number.ToString ()));
	}

    public void OnDoubleZeroButtonPress ()
	{
		SetInputFieldValue (float.Parse (inputFieldValue.ToString () + "00"));
	}

    public void OnClearButtonPress ()
	{
		InitializeVariables ();
	}

    public void OnCancelButtonPress ()
	{
		this.transform.parent.gameObject.SetActive (false);
		this.gameObject.SetActive (false);
	}

    public void OnAcceptButtonPress ()
	{
		itemRow.SetItemPrice (adjustedPrice);
		this.transform.parent.gameObject.SetActive (false);
		if (adjustedPrice != itemRow.GetItemOriginalPrice ()) {
            itemRow.itemPriceText.color = RED;
		} else {
            itemRow.itemPriceText.color = DARK_GREY;
		}
	}

    public void SetItemDescriptionText (string text)
	{
		itemDescription.text = text;
	}

    public void SetInputFieldText (float value)
	{
		if (priceMode == DOLLARS_OFF) {
			float dollarValue = value / 100;
			inputField.text = dollarValue.ToString ("C");
		} else {
			float percentValue = value / 10000; // divide by 100 to make 1000 to 10.00 then divide by 100 to make 10.00 to .1 which will make 10%
			inputField.text = percentValue.ToString ("P");
		}

	}

    private void SetAdjustedPrice (float value)
	{
		adjustedPrice = value;
		adjustedPriceText.text = adjustedPrice.ToString ("C");
	}

    private float GetAdjustedPrice ()
	{
		return adjustedPrice;
	}

    private void SetOriginalPrice (float value)
	{
		originalPrice = value;
		originalPriceText.text = originalPrice.ToString ("C");
	}

    private float GetOriginalPrice ()
	{
		return originalPrice;
	}

    private void SetInputFieldValue (float value)
	{
		inputFieldValue = value;
		float calculatedValue;
		if (priceMode == DOLLARS_OFF) {
			calculatedValue = inputFieldValue / 100;
			inputField.text = calculatedValue.ToString ("C");
		} else {
			calculatedValue = inputFieldValue / 10000;
			inputField.text = calculatedValue.ToString ("P");
		}

		CalculatePriceFields ();
	}

    public void CalculatePriceFields ()
	{
		float calculatedValue = inputFieldValue / 100;
		if (priceMode == DOLLARS_OFF) {
			SetAdjustedPrice (originalPrice - dollarDiscount);
		} else if (priceMode == PERCENT_OFF) {
			SetAdjustedPrice (originalPrice - dollarDiscount);
		} 
	}

    public void OnPriceModeChange ()
	{
        //set all price mode text to black
        foreach (Toggle toggle in modeButtonPanel.GetComponentsInChildren<Toggle>()) {
            if(toggle.isOn) {
                toggle.GetComponentInChildren<Image>().sprite = selectedImage;
            } else {
                toggle.GetComponentInChildren<Image>().sprite = unselectedImage;
            }
        }

        Toggle activeToggle = modeButtonPanel.ActiveToggles().First();
        if (activeToggle.name == "DollarModeToggle") {

        }

		SetInputFieldText (inputFieldValue);
		CalculatePriceFields ();
	}

    public void OnNumberButtonDown() {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = THEME_GREEN; 
        button.GetComponentInChildren<Text>().color = WHITE; 
    }

    public void OnNumberButtonUp() {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = WHITE; 
        button.GetComponentInChildren<Text>().color = THEME_GREEN;
    }
}
