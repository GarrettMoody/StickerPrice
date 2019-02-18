using UnityEngine;
using System.Collections;
using ZXing;
using ZXing.QrCode;

public static class QRCodeGenerator
{ /* This class will be used to house our QR Code generating algorithms. If other classes need 
     a QR code generated, they should use this generic class to do so. */

    public static Texture2D CreateQRCode(string codeString)
    {
        Texture2D encoded = new Texture2D(256, 256);
        Color32[] newCode = Encode(codeString, 256, 256);
        encoded.SetPixels32(newCode);
        encoded.Apply();
        return encoded;
    }

    public static Texture2D CreateQRCode(string codeString, int height, int width)
    {
        Texture2D encoded = new Texture2D(width, height);
        Color32[] newCode = Encode(codeString, width, height);
        encoded.SetPixels32(newCode);
        encoded.Apply();
        return encoded;
    }

    // for generate qrcode
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
