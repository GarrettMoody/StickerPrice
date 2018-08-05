using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using System.IO;


public class QRGenerator : MonoBehaviour
{
	public Texture2D encoded;
	public string Lastresult;

    public RawImage template;

	void Start ()
	{  
		encoded = new Texture2D (256, 256);    
 
		var color32 = Encode ("I Love Sticker Price", encoded.width, encoded.height);  
		encoded.SetPixels32 (color32);  
		encoded.Apply ();

        RawImage [] QRCodes = template.GetComponentsInChildren<RawImage>();
        foreach(RawImage image in QRCodes) {
            if(image.name != "Template") {
                image.texture = encoded;
            }
        }
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

    public void onButtonClicked() {
        StartCoroutine(takeScreenshot());
    }

    public IEnumerator takeScreenshot() {
        yield return new WaitForEndOfFrame();

        int width = System.Convert.ToInt32(template.rectTransform.rect.width);
        int height = System.Convert.ToInt32(template.rectTransform.rect.height);
        Vector2 temp = template.rectTransform.transform.position;
        float startX = temp.x - width / 2;
        float startY = temp.y - height / 2;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "Screenshot.jpg", bytes);
    }
}
