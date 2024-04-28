
using System.Text.Json;
using Colibo.Models;

namespace Colibo.Service;

public class JsonData_Service : IJsonData_Service
{
  private readonly ILogger<JsonData_Service> logger;
  private readonly IMergedData_Service _Service;
  private readonly string path = "MergedData.json";

  public JsonData_Service(ILogger<JsonData_Service> logger, IMergedData_Service _Service)
  {
    this.logger = logger;
    this._Service = _Service;
  }

  public async Task SerializeToJsonAsync(List<MergedUsers> mergedUsers)
  {
    mergedUsers = await _Service.GetAllAsync();
    try
    {
      logger!.LogInformation($"List of Users from service to JSON: {mergedUsers.Count}");
      string json = JsonSerializer.Serialize(mergedUsers, new JsonSerializerOptions { WriteIndented = true });
      await File.WriteAllTextAsync(path, json);
      logger!.LogInformation($"List of Users from service to JSON after serialization: {mergedUsers.Count}");
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error creating JSON file: {ex.Message}");
      throw;
    }
  }
  public async Task<List<MergedUsers>> ReadJsonFileAsync()
  {
    try
    {
      var content = await File.ReadAllTextAsync(path);
      var mergedUsers = JsonSerializer.Deserialize<List<MergedUsers>>(content);

      logger!.LogInformation($"List of Users from service to JSON after serialization: {mergedUsers!.Count}");
      return mergedUsers!;
    }
    catch (Exception ex)
    {
      logger.LogError($"Error reading JSON file: {ex.Message}");
      throw;
    }
  }
  public async Task SaveNewUserAsync(MergedUsers newUser)
  {
    try
    {
      var content = await File.ReadAllTextAsync(path);
      var list = JsonSerializer.Deserialize<List<MergedUsers>>(content);

      if (list != null)
      {
        list.Add(newUser);
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(path, json);

        logger!.LogInformation($"New user added to file successfully: {newUser.Id}, {list.Count}");
      }
      else
      {
        logger!.LogError("Deserialization of JSON content failed or resulted in a null list.");
      }
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error saving new user to JSON file: {ex.Message}");
      throw;
    }
  }

  public async Task DeleteUserAsync(string id)
  {
    try
    {
      var content = await File.ReadAllTextAsync(path);
      var userList = JsonSerializer.Deserialize<List<MergedUsers>>(content);

      if (userList != null)
      {
        var userToRemove = userList.FirstOrDefault(u => u.Id!.Equals(id));
        if (userToRemove != null)
        {
          userList.Remove(userToRemove);
          var json = JsonSerializer.Serialize(userList, new JsonSerializerOptions { WriteIndented = true });
          await File.WriteAllTextAsync(path, json);

          logger!.LogInformation($"User with Id {id} deleted successfully.");
        }
        else
        {
          logger!.LogError($"User with Id {id} not found in the list.");
        }
      }
      else
      {
        logger!.LogError("Deserialization of JSON content failed or resulted in a null list.");
      }
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error deleting user from JSON file: {ex.Message}");
      throw;
    }
  }


  // public async Task UpdateAsync(string id)
  // {
  //   var getAll = await GetAllAsync();
  //   var toUpdate = getAll!.FirstOrDefault(u => u.Id!.Equals(id));
  //   getAll.Remove(toUpdate!);
  //   await jsonData!.SerializeToJsonAsync(pathToJson, getAll);
  // }

  // public async Task<MergedUsers> GetByIdAsync(string id)
  // {
  //   var getAll = await GetAllAsync();
  //   var findUser = getAll!.FirstOrDefault(uid => uid.Id!.Equals(id));
  //   return findUser!;
  // }
}