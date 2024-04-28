using System.Xml.Serialization;

namespace Colibo.Models
{
  [XmlRoot(ElementName = "persons")]
  public class EmployeeData
  {
    [XmlElement(ElementName = "person")]
    public List<Employee> persons = [];
  }
}