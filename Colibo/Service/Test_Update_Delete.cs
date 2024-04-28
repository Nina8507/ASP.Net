using Colibo.Models;
using Colibo.Service.Json_Service;

namespace Colibo.Service;

public class Test_Update_Delete
{
  private readonly MergedUsers users = new();
  private readonly IJsonData_Service _Service;
  private readonly ILogger<Test_Update_Delete> logger;

  public Test_Update_Delete(ILogger<Test_Update_Delete> logger, IJsonData_Service _Service)
  {
    this._Service = _Service;
    this.logger = logger;
  }
}