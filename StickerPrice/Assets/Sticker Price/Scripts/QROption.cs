using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QROption : MonoBehaviour
{

    public Text description;
    public Text productOwner;
    public Text price;
    public RawImage qrCode;
    public Text numberOfStickers;

    public void SetDescription(string value)
    {
        description.text = value;
    }

    public void SetProductionOwner(string value)
    {
        productOwner.text = value;
    }

    public void SetPrice(string value) {
        price.text = value;
    }

    public void SetNumberOfStickers(string value) {
        numberOfStickers.text = value;
    }
}
