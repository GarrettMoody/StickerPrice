using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template
{
    public string templateId;
    public string size;
    public string qrCode;
    public string numberPerSheet;

    public Template()
    {
        templateId = "";
        size = "";
        qrCode = "";
        numberPerSheet = "";
    }

    public Template(string templateId)
    {
        this.templateId = templateId;
        size = "";
        qrCode = "";
        numberPerSheet = "";
    }

    public Template(string templateId, string size, string qrCode, string numberPerSheet)
    {
        this.templateId = templateId;
        this.size = size;
        this.qrCode = qrCode;
        this.numberPerSheet = numberPerSheet;
    }
}
