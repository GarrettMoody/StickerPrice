using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StickerPageData : MonoBehaviour
{
    private string filePath = "Assets/Sticker Price/Data Files/SavedStickerPages.json";
    private FileUtility fileUtility = new FileUtility();
    private List<StickerPage> stickerPageList = new List<StickerPage>();

    //public StickerPageData()
    //{
    //    ReadStickerPages();
    //}

    //private void WriteStickerPages()
    //{
    //    fileUtility.ClearFile(filePath);
    //    fileUtility.WriteJson(filePath, JsonConvert.SerializeObject(stickerPageList, Formatting.Indented));
    //}

    //private void ReadStickerPages()
    //{
    //    stickerPageList = JsonConvert.DeserializeObject<List<StickerPage>>(fileUtility.ReadJson(filePath));
    //    if (stickerPageList == null)
    //    {
    //        stickerPageList = new List<StickerPage>();
    //    }
    //}

    //private void RemoveDuplicate(StickerPage stickerPages)
    //{
    //    ReadStickerPages();
    //    if (stickerPageList.Exists(x => x.stickerName == sticker.stickerName))
    //    {
    //        //Get the existing transacation and remove it from the list
    //        Sticker removeSticker = stickerPageList.Single(x => x.stickerName == sticker.stickerName);
    //        stickerPageList.Remove(removeSticker);
    //    }
    //}

    //public void AddSticker(StickerPage stickerPage)
    //{
    //    RemoveDuplicate(stickerPage);
    //    stickerPageList.Add(stickerPage);
    //    WriteStickerPages();
    //}

    //public void DeleteSticker(StickerPage stickerPage)
    //{
    //    RemoveDuplicate(stickerPage);
    //    WriteStickerPages();
    //}

    //public List<StickerPage> GetAllStickers()
    //{
    //    ReadStickerPages();
    //    return stickerPageList;
    //}
}
