using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class TemplateData
{
    private string filePath = "Assets/Sticker Price/Data Files/Templates.json";
    private FileUtility fileUtility = new FileUtility();
    private List<Template> templateList = new List<Template>();

    public TemplateData ()
    {
        ReadTemplates();
    }

    private void ReadTemplates()
    {
        templateList = JsonConvert.DeserializeObject<List<Template>>(fileUtility.ReadJson(filePath));
    }

    public List<Template> GetAllTemplates()
    {
        ReadTemplates();
        return templateList;
    }

}
