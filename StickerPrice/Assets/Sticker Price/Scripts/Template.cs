using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template {
    public string title;
    public string size;
    public int numPerSheet;
    public QRCode qrCode;

    public Template (string title,string size,int numPerSheet,QRCode qrCode) 
    {
        this.title = title;
        this.size = size;
        this.numPerSheet = numPerSheet;
        this.qrCode = qrCode;
    }

}
