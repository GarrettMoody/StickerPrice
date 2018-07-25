using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;

public class QRGenerator : MonoBehaviour
{

	public Texture2D encoded;
	public string Lastresult;

	//public RawImage ima;

	void Start ()
	{  
		encoded = new Texture2D (256, 256);    
 
		var color32 = Encode ("I Love Sticker Price", encoded.width, encoded.height);  
		encoded.SetPixels32 (color32);  
		encoded.Apply ();  

		//ima.texture = encoded;  

	}

	// for generate qrcode
	private static Color32[] Encode (string textForEncoding, int width, int height)
	{  
		var writer = new BarcodeWriter {  
			Format = BarcodeFormat.QR_CODE,  
			Options = new QrCodeEncodingOptions {  
				Height = height,  
				Width = width  
			}  
		};  
		return writer.Write (textForEncoding);  
	}

    public void onButtonClick() {
        PdfDocument pdf = PdfReader.Open("SampleTest.pdf", PdfDocumentOpenMode.Modify);
        if(pdf != null) {
            PdfTextField textField = (PdfTextField)pdf.AcroForm.Fields["Text1"];
            if (textField != null)
            {
                PdfString newValue = new PdfString("Hello World");
                textField.Value = newValue;

                pdf.Save("SampleTest.pdf");
                pdf.Close();
            }
        }
    }
}
