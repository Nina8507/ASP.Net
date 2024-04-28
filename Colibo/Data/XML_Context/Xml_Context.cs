using System.Xml.Serialization;
using Colibo.Models;

namespace Colibo.Data.XML_Context;

public class Xml_Context(ILogger<Xml_Context> logger) : IContext_Xml
{
  private readonly string xml_Persons = "Payroll.xml";
  private readonly ILogger<Xml_Context> logger = logger;
  private readonly XmlSerializer serializer = new(typeof(EmployeeData));

  public async Task<EmployeeData> Initialize_xml_Async()
  {
    return await ReadDataAsync(xml_Persons);
  }

  private async Task<EmployeeData> ReadDataAsync(string xml_Persons)
  {
    try
    {
      var xmlContent = await File.ReadAllTextAsync(xml_Persons);
      var persons = (EmployeeData?)serializer.Deserialize(new StringReader(xmlContent));
      logger.LogInformation($"Read data from the XML: {persons!.persons.Count}");
      return persons!;
    }
    catch (Exception e)
    {
      logger.LogError($"Read data from xml: {e.StackTrace}");
      throw;
    }
  }
}
