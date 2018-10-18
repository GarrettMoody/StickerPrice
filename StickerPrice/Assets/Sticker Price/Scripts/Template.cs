using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Template : MonoBehaviour {
    public Text title;
    public Text size;
    public Text numPerSheet;
    public Image qrCode;

    public void Start()
    {
        
    }

    public void setTitle(string title)
    {
        this.title.text = title;
    }

    public void setSize(string size)
    {
        this.size.text = size;
    }

    public void setNumPerSheet (string numPerSheet)
    {
        this.numPerSheet.text = numPerSheet;
    }

    public void initializeVariables(string title, string size, string numPerSheet)
    {
        setTitle(title);
        setSize(size);
        setNumPerSheet(numPerSheet);
    }

}
