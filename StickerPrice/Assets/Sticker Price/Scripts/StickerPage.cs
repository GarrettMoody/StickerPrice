using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class StickerPage
{
    public List<Sticker> stickers = new List<Sticker>();
    public Template template;
    private int numberOfPages;
    private int stickersLeftOnPage;

    public StickerPage (Template template)
    {
        this.template = template;
    }

    public void AddSticker(Sticker sticker)
    {
        //See if sticker already exists
        Sticker findSticker = stickers.FirstOrDefault(x => x.itemDescription == sticker.itemDescription
                             && x.owner == sticker.owner
                             && x.price == sticker.price
                             && x.template == sticker.template);

        if (findSticker != null)
        {
            //If sticker exists, add quantity to existing sticker
            int currentQuantity = findSticker.quantity;
            int newQuantity = currentQuantity + sticker.quantity;
            findSticker.quantity = newQuantity;
        }
        else
        {
            //else add the sticker to the list
            stickers.Add(sticker);
        }

        UpdateNumberOfPages();
    }

    private void UpdateNumberOfPages()
    {
        if(GetNumberOfStickers() == 0)
        {
            numberOfPages = 1;
        }
        else
        {
            numberOfPages = (int)Math.Ceiling((decimal)GetNumberOfStickers() / (decimal)GetTemplateNumberPerSheet());
        }

    }

    public int GetTemplateNumberPerSheet()
    {
        if (template != null)
        {
            return int.Parse(!string.IsNullOrEmpty(template.numberPerSheet) ? template.numberPerSheet : "0");
        }
        else
        {
            return 0;
        }
    }

    public int GetNumberOfPages()
    {
        UpdateNumberOfPages();
        return numberOfPages;
    }

    public void UpdateStickersLeftOnPage()
    {
        UpdateNumberOfPages();
        stickersLeftOnPage = (numberOfPages * GetTemplateNumberPerSheet()) - GetNumberOfStickers();
    }

    public int GetStickersLeftOnPage()
    {
        UpdateStickersLeftOnPage();
        return stickersLeftOnPage;
    }

    public int GetNumberOfStickers()
    {
        int num = 0;
        foreach(Sticker sticker in stickers)
        {
            num += sticker.quantity;
        }
        return num;
    }

}
