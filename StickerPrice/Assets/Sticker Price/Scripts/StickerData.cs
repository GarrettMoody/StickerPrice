using System.Collections.Generic;
using Newtonsoft.Json;

public class StickerData {

    public string stickerDescription;
    public string itemDescription;
    public string price;
    public string dateSaved;
    public string owner;
    public string quantity;
    public string templateId;

    private string filePath = "Assets/Sticker Price/Data Files/SavedStickers.json";
    private FileUtility fileUtility = new FileUtility();
    private List<StickerData> stickerList = new List<StickerData>();

    public StickerData()
    {
        stickerDescription = "";
        itemDescription = "";
        price = "";
        dateSaved = "";
        owner = "";
        quantity = "";
        templateId = "";
    }

    public StickerData(string stickerDescription, string itemDescription, string price, string dateSaved, string owner, string quantity, string templateId)
    {
        this.stickerDescription = stickerDescription;
        this.itemDescription = itemDescription;
        this.price = price;
        this.dateSaved = dateSaved;
        this.owner = owner;
        this.quantity = quantity;
        this.templateId = templateId;
    }

    public void writeStickers ()
    {
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(stickerList));
    }

    public void readStickers()
    {
        stickerList = JsonConvert.DeserializeObject<List<StickerData>>(fileUtility.readJson(filePath));
    }

    public void removeDuplicate ()
    {
        readStickers();
        List<StickerData> newList = new List<StickerData>();
        if (stickerList != null && stickerList.Count > 0)
        {
            stickerList.ForEach(delegate (StickerData sticker)
            {
                if (sticker.stickerDescription != this.stickerDescription)
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
         stickerList.Add(this);
         writeStickers();        
    }

    public void deleteSticker()
    {
        removeDuplicate();
        writeStickers();
    }

    public List<StickerData> getAllStickers()
    {
        readStickers();
        return stickerList;
    }
}
