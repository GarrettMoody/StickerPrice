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

    public void setDescription(string value)
    {
        description.text = value;
    }

    public void setProductionOwner(string value)
    {
        productOwner.text = value;
    }

    public void setPrice(string value) {
        price.text = value;
    }
}
