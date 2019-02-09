using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class TemplateData
{
    private string filePath = "Assets/Sticker Price/Data Files/Templates.json";
    private FileUtility fileUtility = new FileUtility();
    private List<Template> templateList = new List<Template>();
    private Template newTemplate = new Template();

    public TemplateData(Template newTemplate)
    {
        this.newTemplate = newTemplate;
    }

    public TemplateData ()
    {

    }

    public void writeTemplates()
    {
        fileUtility.clearFile(filePath);
        fileUtility.writeJson(filePath, JsonConvert.SerializeObject(templateList));
    }

    public void readTemplates()
    {
        templateList = JsonConvert.DeserializeObject<List<Template>>(fileUtility.readJson(filePath));
    }

    public void removeDuplicate()
    {
        readTemplates();
        List<Template> newList = new List<Template>();
        if (templateList != null && templateList.Count > 0)
        {
            templateList.ForEach(delegate (Template template)
            {
                if (template.templateId != newTemplate.templateId)
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
        templateList.Add(newTemplate);
        writeTemplates();
    }

    public void deleteTemplate()
    {
        removeDuplicate();
        writeTemplates();
    }

    public List<Template> getAllTemplates()
    {
        readTemplates();
        return templateList;
    }

    public Template getTemplate()
    {
        Template templateData = new Template();
        readTemplates();
        if (templateList != null && templateList.Count > 0)
        {
            templateList.ForEach(delegate (Template template)
            {
                if (template.templateId == newTemplate.templateId)
                {
                    templateData = template;
                }
            });
        }
        return templateData;
    }
}
