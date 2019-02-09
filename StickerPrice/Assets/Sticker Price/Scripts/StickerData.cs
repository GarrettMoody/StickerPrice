using System.Collections.Generic;
using Newtonsoft.Json;

public class StickerData {

    private string filePath = "Assets/Sticker Price/Data Files/SavedStickers.json";
    private FileUtility fileUtility = new FileUtility();
    private List<Sticker> stickerList = new List<Sticker>();
    private Sticker newSticker;

    public StickerData (Sticker newSticker)
    {
        this.newSticker = newSticker;
    }

    public void writeStickers ()
    {
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(stickerList));
    }

    public void readStickers()
    {
        stickerList = JsonConvert.DeserializeObject<List<Sticker>>(fileUtility.readJson(filePath));
    }

    public void removeDuplicate ()
    {
        readStickers();
        List<Sticker> newList = new List<Sticker>();
        if (stickerList != null && stickerList.Count > 0)
        {
            stickerList.ForEach(delegate (Sticker sticker)
            {
                if (sticker.stickerDescription != newSticker.stickerDescription)
                {
                    newList.Add(sticker);
                }
            });
        }
        stickerList = newList;
    }

    public void createSticker()
    {
         removeDuplicate();
         stickerList.Add(newSticker);
         writeStickers();        
    }

    public void deleteSticker()
    {
        removeDuplicate();
        writeStickers();
    }

    public List<Sticker> getAllStickers()
    {
        readStickers();
        return stickerList;
    }
}
