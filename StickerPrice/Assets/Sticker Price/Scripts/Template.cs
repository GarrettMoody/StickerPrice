﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Template
{
    public string templateId;
    public string size;
    public string qrCode;
    public string numberPerSheet;   

    public Template ()
    {
        this.templateId = "";
        this.size = "";
        this.qrCode = "";
        this.numberPerSheet = "";
    }

    public Template(string templateId)
    {
        this.templateId = templateId;
        this.size = "";
        this.qrCode = "";
        this.numberPerSheet = "";
    }
    
    public Template(string templateId, string size, string qrCode, string numberPerSheet)
    {
        this.templateId = templateId;
        this.size = size;
        this.qrCode = qrCode;
        this.numberPerSheet = numberPerSheet;
    }
}