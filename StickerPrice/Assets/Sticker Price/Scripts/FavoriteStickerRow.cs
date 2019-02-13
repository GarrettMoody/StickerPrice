using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FavoriteStickerRow : ContentRow
{
    [Header("Favorite Sticker Row Properties")]
    public TextMeshProUGUI stickerName;
    public TextMeshProUGUI templateNumber;
    public TextMeshProUGUI stickerSize;
    public TextMeshProUGUI dateTime;

    public override void DefaultButtonOnClickListener()
    {
        return;
    }
}
