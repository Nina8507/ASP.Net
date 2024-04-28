using System.Xml.Serialization;

namespace Colibo.Models;

[XmlRoot(ElementName = "person")]
public class Employee
{
  [XmlAttribute("number")]
  public string? EmployeeId { get; set; }
  [XmlElement(ElementName = "name")]
  public string? Name { get; set; }
  [XmlElement(ElementName = "email")]
  public string? Email { get; set; }
  [XmlElement(ElementName = "mobile")]
  public string? Mobile { get; set; }
  [XmlElement(ElementName = "title")]
  public string? Title { get; set; }
  [XmlElement(ElementName = "address")]
  public string? Address { get; set; }
  [XmlElement(ElementName = "city")]
  public string? City { get; set; }
}
