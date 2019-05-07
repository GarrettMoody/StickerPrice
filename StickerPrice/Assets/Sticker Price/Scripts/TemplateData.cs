using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine;
using System.IO;

public class TemplateData
{
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Templates.json");
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
