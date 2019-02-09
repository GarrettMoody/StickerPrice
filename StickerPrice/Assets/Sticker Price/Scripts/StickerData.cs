using System.Collections.Generic;
using Newtonsoft.Json;

public class StickerData {

    private readonly string filePath = "Assets/Sticker Price/Data Files/SavedStickers.json";
    private FileUtility fileUtility = new FileUtility();
    private List<Sticker> stickerList = new List<Sticker>();
    private Sticker newSticker;

    public StickerData ()
    {
        ReadStickers();
    }

    public void WriteStickers()
    {
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(stickerList));
    }

    public void ReadStickers()
    {
        stickerList = JsonConvert.DeserializeObject<List<Sticker>>(fileUtility.readJson(filePath));
    }

    public void RemoveDuplicate()
    {
        ReadStickers();
        List<Sticker> newList = new List<Sticker>();
        if (stickerList != null && stickerList.Count > 0)
        {
            stickerList.ForEach(delegate (Sticker sticker)
            {
                if (sticker.stickerName != newSticker.stickerName)
                {
                    newList.Add(sticker);
                }
            });
        }
        stickerList = newList;
    }

    public void CreateSticker()
    {
        RemoveDuplicate();
         stickerList.Add(newSticker);
        WriteStickers();        
    }

    public void DeleteSticker()
    {
        RemoveDuplicate();
        WriteStickers();
    }

    public List<Sticker> GetStickers()
    {
        return stickerList;
    }
}
