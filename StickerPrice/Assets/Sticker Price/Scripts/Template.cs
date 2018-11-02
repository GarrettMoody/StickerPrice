using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Template : MonoBehaviour {
    public Text title;
    public Text size;
    public Text numPerSheet;
    public Image qrCode;
    public string templateId;
    public string numberPerSheet;

    public void Start()
    {
        
    }

    public void setTitle()
    {
        title.text = "Template - " + templateId;
    }

    public void setSize(string size)
    {
        this.size.text = size;
    }

    public void setNumPerSheet ()
    {
        this.numPerSheet.text = numberPerSheet + " Per Sheet";
    }

    public void initializeVariables(string templateId, string size, string numberPerSheet)
    {
        this.templateId = templateId;
        this.numberPerSheet = numberPerSheet;
        setTitle();
        setSize(size);
        setNumPerSheet();
    }

}
