using Colibo.Models;

namespace Colibo.Data.XML_Context
{
  public interface IContext_Xml
  {
    Task<EmployeeData> Initialize_xml_Async();
  }
}