using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRCode {

    public string description;
    public string productOwner;
    public string price;
    public string qrCode;

    public QRCode(string description, string productOwner, string price, string qrCodeImage)
    {
        setDescription(description);
        setProductionOwner(productOwner);
        setPrice(price);
        setQrCode(qrCodeImage);
    }

    public void setDescription(string value)
    {
        description = value;
    }

    public void setProductionOwner(string value)
    {
        productOwner= value;
    }

    public void setPrice(string value)
    {
        price = value;

    }

    public void setQrCode(string image)
    {
        // load texture from resource folder
        //Texture2D myTexture = Resources.Load("Graphics/Individual Elements/" + image) as Texture2D;
        qrCode = image;
    }
}
