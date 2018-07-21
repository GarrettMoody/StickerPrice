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
	public Text percentDiscountText;
	public Text dollarDiscountText;
	public Text adjustedPriceText;

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
	private const string DOLLARS_SET = "SetDollars";
	private const string PERCENT_OFF = "PercentOff";
	private const string PERCENT_SET = "SetPercent";

	public void openAdjustPanel (ItemRow row)
	{
		itemRow = row;
		initializeVariables ();
		this.transform.parent.gameObject.SetActive (true);
		this.gameObject.SetActive (true);
	}

	private void initializeVariables ()
	{
		setItemDescriptionText (itemRow.getItemDescription ());
		setOriginalPrice (itemRow.getItemOriginalPrice ());
		setInputFieldValue (0);
		adjustedPrice = originalPrice;
	}

	public void onNumberButtonPress (int number)
	{
		setInputFieldValue (float.Parse (inputFieldValue.ToString () + number.ToString ()));
	}

	public void onDoubleZeroButtonPress ()
	{
		setInputFieldValue (float.Parse (inputFieldValue.ToString () + "00"));
	}

	public void onClearButtonPress ()
	{
		initializeVariables ();
	}

	public void onCancelButtonPress ()
	{
		this.transform.parent.gameObject.SetActive (false);
		this.gameObject.SetActive (false);
	}

	public void onAcceptButtonPress ()
	{
		itemRow.setItemPrice (adjustedPrice);
		this.transform.parent.gameObject.SetActive (false);
		if (adjustedPrice != itemRow.getItemOriginalPrice ()) {
			itemRow.itemPriceText.color = new Color32 (0xE2, 0x23, 0x1A, 0xFF);
		} else {
			itemRow.itemPriceText.color = new Color32 (0x52, 0x53, 0x49, 0xFF);
		}
	}

	public void setItemDescriptionText (string text)
	{
		itemDescription.text = text;
	}

	public void setInputFieldText (float value)
	{
		if (priceMode == DOLLARS_SET || priceMode == DOLLARS_OFF) {
			float dollarValue = value / 100;
			inputField.text = dollarValue.ToString ("C");
		} else {
			float percentValue = value / 10000; // divide by 100 to make 1000 to 10.00 then divide by 100 to make 10.00 to .1 which will make 10%
			inputField.text = percentValue.ToString ("P");
		}

	}

	private void setAdjustedPrice (float value)
	{
		adjustedPrice = value;
		adjustedPriceText.text = adjustedPrice.ToString ("C");
	}

	private float getAdjustedPrice ()
	{
		return adjustedPrice;
	}

	private void setOriginalPrice (float value)
	{
		originalPrice = value;
		originalPriceText.text = originalPrice.ToString ("C");
	}

	private float getOriginalPrice ()
	{
		return originalPrice;
	}

	private void setDollarDiscount (float value)
	{
		dollarDiscount = value;
		dollarDiscountText.text = dollarDiscount.ToString ("C");
	}

	private float getDollarDisount ()
	{
		return dollarDiscount;
	}

	private void setPercentDiscount (float value)
	{
		percentDiscount = value;
		percentDiscountText.text = "  " + percentDiscount.ToString ("P1");
	}

	private float getPercentDiscount ()
	{
		return percentDiscount;
	}

	private void setInputFieldValue (float value)
	{
		inputFieldValue = value;
		float calculatedValue;
		if (priceMode == DOLLARS_OFF || priceMode == DOLLARS_SET) {
			calculatedValue = inputFieldValue / 100;
			inputField.text = calculatedValue.ToString ("C");
		} else {
			calculatedValue = inputFieldValue / 10000;
			inputField.text = calculatedValue.ToString ("P");
		}

		calculatePriceFields ();
	}

	public void calculatePriceFields ()
	{
		float calculatedValue = inputFieldValue / 100;
		if (priceMode == DOLLARS_OFF) {
			setDollarDiscount (calculatedValue);
			setPercentDiscount (dollarDiscount / originalPrice);
			setAdjustedPrice (originalPrice - dollarDiscount);
		} else if (priceMode == DOLLARS_SET) {
			setDollarDiscount (originalPrice - calculatedValue);
			setPercentDiscount (dollarDiscount / originalPrice);
			setAdjustedPrice (calculatedValue);
		} else if (priceMode == PERCENT_OFF) {
			setDollarDiscount (calculatedValue / 100 * originalPrice);
			setPercentDiscount (calculatedValue / 100);
			setAdjustedPrice (originalPrice - dollarDiscount);
		} else if (priceMode == PERCENT_SET) {
			setDollarDiscount ((100 - calculatedValue) / 100 * originalPrice);
			setPercentDiscount ((100 - calculatedValue) / 100);
			setAdjustedPrice (originalPrice - dollarDiscount);
		} 
	}

	public void onPriceModeChange ()
	{
        //set all price mode text to black
        foreach (Toggle toggle in modeButtonPanel.GetComponentsInChildren<Toggle>()) {
            toggle.GetComponentInChildren<Text>().color = new Color32(0, 0, 0, 255); //black
        }
		IEnumerable<Toggle> activeToggle = modeButtonPanel.ActiveToggles ();

        //set selected price mode text to white
        activeToggle.First().GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255); //white
		priceMode = activeToggle.First ().name;

		setInputFieldText (inputFieldValue);
		calculatePriceFields ();
	}

    public void onNumberButtonDown() {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = new Color32(0x3F, 0xAE, 0x2A, 0xFF); //theme green
        button.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255); //white
    }

    public void onNumberButtonUp() {
        Button button = (Button)EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        button.GetComponent<Image>().color = new Color32(255, 255, 255, 255); //white
        button.GetComponentInChildren<Text>().color = new Color32(0x3F, 0xAE, 0x2A, 0xFF); //theme green
    }
}
