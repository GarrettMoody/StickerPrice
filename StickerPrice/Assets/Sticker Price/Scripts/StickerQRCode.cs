﻿using UnityEngine;
using ZXing;
using ZXing.QrCode;

public static class StickerQRCode
{
    public static Texture2D CreateQRCode(Sticker sticker)
    {
        Texture2D encoded = new Texture2D(256, 256);
        Color32[] newCode = Encode("|" + sticker.price + "|" + sticker.itemDescription + "|" + sticker.owner, 256, 256);
        encoded.SetPixels32(newCode);
        encoded.Apply();
        return encoded;
    }

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