using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public void OnTemplate22805Clicked() {
        stickerDetailMenu.OpenMenu(22805);
    }

    public void OnTemplate6450Clicked() {
        stickerDetailMenu.OpenMenu(6450);
    }

    public void OnTemplate1234Clicked() {
        stickerDetailMenu.OpenMenu(1234);
    }
}
