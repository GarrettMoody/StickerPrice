using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class TemplateData
{
    public string templateId;
    public string size;
    public string qrCode;
    public string numberPerSheet;

    private string filePath = "Assets/Sticker Price/Data Files/Templates.json";
    private FileUtility fileUtility = new FileUtility();
    private List<TemplateData> templateList = new List<TemplateData>();

    public TemplateData()
    {
        templateId = "";
        size = "";
        qrCode = "";
        numberPerSheet = "";
    }

    public TemplateData(string templateId)
    {
        this.templateId = templateId;
        size = "";
        qrCode = "";
        numberPerSheet = "";
    }

    public TemplateData(string templateId, string size, string qrCode, string numberPerSheet)
    {
        this.templateId = templateId;
        this.size = size;
        this.qrCode = qrCode;
        this.numberPerSheet = numberPerSheet;
    }

    public void writeTemplates()
    {
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(templateList));
    }

    public void readTemplates()
    {
        templateList = JsonConvert.DeserializeObject<List<TemplateData>>(fileUtility.readJson(filePath));
    }

    public void removeDuplicate()
    {
        readTemplates();
        List<TemplateData> newList = new List<TemplateData>();
        if (templateList != null && templateList.Count > 0)
        {
            templateList.ForEach(delegate (TemplateData template)
            {
                if (template.templateId != this.templateId)
                {
                    newList.Add(template);
                }
            });
        }
        templateList = newList;
    }

    public void createTemplate()
    {
        removeDuplicate();
        templateList.Add(this);
        writeTemplates();
    }

    public void deleteTemplate()
    {
        removeDuplicate();
        writeTemplates();
    }

    public List<TemplateData> getAllTemplates()
    {
        readTemplates();
        return templateList;
    }

    public TemplateData getTemplate()
    {
        TemplateData templateData = new TemplateData();
        readTemplates();
        if (templateList != null && templateList.Count > 0)
        {
            templateList.ForEach(delegate (TemplateData template)
            {
                if (template.templateId == this.templateId)
                {
                    templateData = template;
                }
            });
        }
        return templateData;
    }
}
