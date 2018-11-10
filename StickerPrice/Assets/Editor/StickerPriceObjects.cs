using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StickerPriceObjects : MonoBehaviour {

    [MenuItem("GameObject/UI/StickerPrice/Content Scroll")]
    private static void CreateContentScroll() {
        ContentScroll contentScrollPrefab = (ContentScroll)AssetDatabase.LoadAssetAtPath("Assets/Sticker Price/Prefabs/ContentScroll.prefab", typeof(ContentScroll));
        ContentScroll newScroll = Instantiate<ContentScroll>(contentScrollPrefab);
        newScroll.transform.SetParent(Selection.activeGameObject.transform);
    }
}
