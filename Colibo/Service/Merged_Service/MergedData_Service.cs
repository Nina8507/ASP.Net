using Colibo.Data.AZURE_Context;
using Colibo.Data.XML_Context;
using Colibo.Models;


namespace Colibo.Service.MergedData_Service;

public class MergedData_Service : IMergedData_Service
{
  public List<MergedUsers> mergedUsers = [];
  private readonly ILogger<MergedData_Service> logger;
  private readonly IContext_Xml _Xml;
  private readonly IContext_Azure _Azure;

  public MergedData_Service(IContext_Azure _Azure, IContext_Xml _Xml, ILogger<MergedData_Service> logger)
  {
    this._Xml = _Xml;
    this._Azure = _Azure;
    this.logger = logger;
  }

  public async Task<List<MergedUsers>> GetAllAsync()
  {
    return await MergedUsersAsync();
  }

  private async Task<List<MergedUsers>> MergedUsersAsync()
  {
    var users = await _Azure.Initialize_Azure_Async();
    var employees = await _Xml.Initialize_xml_Async();

    List<Employee> empXml = employees.persons;
    List<User> userAzure = users.Value!;

    logger.LogInformation($"Employee list from XML is size: {empXml.Count}");
    logger.LogInformation($"User list from Azure is size: {userAzure.Count}");

    mergedUsers = MergeLists(empXml, userAzure);

    logger.LogInformation($"Merged list from both sources is size: {userAzure.Count}");

    return mergedUsers;
  }

  private List<MergedUsers> MergeLists(List<Employee> empXml, List<User> userAzure)
  {
    var mergedLists = empXml.Join(userAzure!, emp => emp.Name, usr => usr.DisplayName,
          (emp, usr) => new MergedUsers
          {
            Id = usr.Id,
            FullName = emp.Name,
            Email = emp.Email ?? usr.UserPrincipalName,
            JobTitle = emp.Title ?? usr.JobTitle,
            Mobile = emp.Mobile ?? usr.MobilePhone,
            Address = emp.Address,
            City = emp.City
          })
    .ToList();

    var remainingUsers = userAzure!.Where(u => !mergedLists.Any(mu => mu.FullName == u.DisplayName))
        .Select(u => new MergedUsers
        {
          Id = u.Id,
          FullName = u.DisplayName,
          Email = u.UserPrincipalName,
          JobTitle = u.JobTitle,
          Mobile = u.MobilePhone
        });

    mergedLists.AddRange(remainingUsers);

    return mergedLists;
  }
}