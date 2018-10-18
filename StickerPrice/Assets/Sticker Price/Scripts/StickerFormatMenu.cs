using UnityEngine;
using UnityEngine.UI;

public class StickerFormatMenu : MonoBehaviour {

    public StickerDetailMenu stickerDetailMenu;
    public Template[] templateList;
    public ScrollRect scrollView;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;

    // Use this for initialization
    void Awake()
    {
        templateList = new Template[] {new Template("Template - 22805", "1.5 x 1.5", 24, new QRCode("Description", "Owner", "$0.00", "QR Code 1")),
                                       new Template("Template - 6450", "1 x 1", 63, new QRCode("Description", "Owner", "$0.00", "QR Code 1")),
                                       new Template("Template - 1234", "1 x 1.75", 30, new QRCode("Description", "Owner", "$0.00", "QR Code 1"))};
        for (int i = 0; i < templateList.Length; i ++)
        {
            generateTemplates(templateList[i]);
        }
        scrollView.verticalNormalizedPosition = 1;
    }

    public void OnTemplate22805Clicked() {
        stickerDetailMenu.OpenMenu(22805);
    }

    public void OnTemplate6450Clicked() {
        stickerDetailMenu.OpenMenu(6450);
    }

    public void OnTemplate1234Clicked() {
        stickerDetailMenu.OpenMenu(1234);
    }

    public void generateTemplates(Template template)
    {
        GameObject scrollItemObj = Instantiate(scrollItemPrefab);
        scrollItemObj.transform.SetParent(scrollContent.transform, false);
        scrollItemObj.transform.Find("Title").gameObject.GetComponent<Text>().text = template.title;
        scrollItemObj.transform.Find("Size").gameObject.GetComponent<Text>().text = template.size;
        scrollItemObj.transform.Find("Number Of Stickers").gameObject.GetComponent<Text>().text = template.numPerSheet.ToString();
    }
}
