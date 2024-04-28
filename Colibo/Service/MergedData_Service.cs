using Colibo.Data.AZURE_Context;
using Colibo.Data.XML_Context;
using Colibo.Models;


namespace Colibo.Service;

public class MergedData_Service : IMergedData_Service
{
  public List<MergedUsers> mergedUsers = [];
  private readonly ILogger<MergedData_Service>? logger;
  private readonly IJsonData_Service? jsonData;
  private readonly string pathToJson = "MergedData.json";
  private readonly IContext_Xml? _Xml;
  private readonly IContext_Azure? _Azure;

  public MergedData_Service(IContext_Azure _Azure, IContext_Xml _Xml, IJsonData_Service jsonData, ILogger<MergedData_Service> logger)
  {
    this._Xml = _Xml;
    this._Azure = _Azure;
    this.jsonData = jsonData;
    this.logger = logger;
  }

  public async Task<List<MergedUsers>> GetAllAsync()
  {
    return await MergedUsersAsync();
  }

  private async Task<List<MergedUsers>> MergedUsersAsync()
  {
    var users = await _Azure!.Initialize_Azure_Async();
    var employees = await _Xml!.Initialize_xml_Async();

    List<Employee>? empXml = employees.persons;
    List<User>? userAzure = users.Value;

    logger!.LogInformation($"Employee list from XML is size: {empXml?.Count}");
    logger!.LogInformation($"User list from Azure is size: {userAzure?.Count}");

    MergeLists(empXml!, userAzure);

    logger!.LogInformation($"Merged list from both sources is size: {userAzure?.Count}");

    await SerializeAsync();
    return mergedUsers;
  }

  private void MergeLists(List<Employee> empXml, List<User>? userAzure)
  {
    foreach (var emp in empXml!)
    {
      var user = userAzure!.FirstOrDefault(u => u.DisplayName == emp.Name);
        if (user != null)
          {
            mergedUsers.Add(new MergedUsers
            {
              Id = user.Id,
              FullName = emp.Name,
              Email = emp.Email ?? user.UserPrincipalName,
              JobTitle = emp.Title ?? user.JobTitle,
              Mobile = emp.Mobile ?? user.MobilePhone,
              Address = emp.Address,
              City = emp.City
            });
          }
    }

    var remainingUsers = userAzure!.Where(u => !mergedUsers.Any(mu => mu.FullName == u.DisplayName));
    foreach (var user in remainingUsers)
    {
      mergedUsers.Add(new MergedUsers
      {
        Id = user.Id,
        FullName = user.DisplayName,
        Email = user.UserPrincipalName,
        JobTitle = user.JobTitle,
        Mobile = user.MobilePhone
      });
    }
  }

  public async Task SerializeAsync()
  {
    logger!.LogInformation($"Json list size is: {mergedUsers.Count}");
    await jsonData!.SerializeToJsonAsync(pathToJson, mergedUsers);
  }


  public async Task AddAync(MergedUsers newUser)
  {
    logger!.LogInformation($"Service add new employee with Id: {newUser.Id}");
    mergedUsers.Add(newUser);
    logger!.LogInformation($"Service new mergedData list is: {mergedUsers.Count}");
    await jsonData!.SaveChangesAsync(pathToJson, mergedUsers);
  }

  public async Task DeleteAsync(string id)
  {
    var getAll = await GetAllAsync();
    var toRemove = getAll!.FirstOrDefault(u => u.Id!.Equals(id));
    getAll.Remove(toRemove!);
    await jsonData!.SaveChangesAsync(pathToJson, getAll);
  }

  public async Task UpdateAsync(string id)
  {
    var getAll = await GetAllAsync();
    var toUpdate = getAll!.FirstOrDefault(u => u.Id!.Equals(id));
    getAll.Remove(toUpdate!);
    await jsonData!.SaveChangesAsync(pathToJson, getAll);
  }

  public async Task<MergedUsers> GetByIdAsync(string id)
  {
    var getAll = await GetAllAsync();
    var findUser=  getAll!.FirstOrDefault(uid => uid.Id!.Equals(id));
    return findUser!;
  }
}