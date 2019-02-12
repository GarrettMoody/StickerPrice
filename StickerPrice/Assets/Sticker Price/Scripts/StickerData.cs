using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class StickerData {

    private string filePath = "Assets/Sticker Price/Data Files/SavedStickers.json";
    private FileUtility fileUtility = new FileUtility();
    private List<Sticker> stickerList = new List<Sticker>();

    public StickerData ()
    {
        ReadStickers();
    }

    private void WriteStickers()
    {
        fileUtility.ClearFile(filePath);
        fileUtility.WriteJson(filePath, JsonConvert.SerializeObject(stickerList, Formatting.Indented));
    }

    private void ReadStickers()
    {
        stickerList = JsonConvert.DeserializeObject<List<Sticker>>(fileUtility.ReadJson(filePath));
        if (stickerList == null)
        {
            stickerList = new List<Sticker>();
        }
    }

    private void RemoveDuplicate(Sticker sticker)
    {
        ReadStickers();
        if (stickerList.Exists(x => x.stickerName == sticker.stickerName))
        {
            //Get the existing transacation and remove it from the list
            Sticker removeSticker = stickerList.Single(x => x.stickerName == sticker.stickerName);
            stickerList.Remove(removeSticker);
        }
    }

    public void AddSticker(Sticker sticker)
    {
        RemoveDuplicate(sticker);
        stickerList.Add(sticker);
        WriteStickers();        
    }

    public void DeleteSticker(Sticker sticker)
    {
        RemoveDuplicate(sticker);
        WriteStickers();
    }

    public List<Sticker> GetAllStickers()
    {
        ReadStickers();
        return stickerList;
    }
}
