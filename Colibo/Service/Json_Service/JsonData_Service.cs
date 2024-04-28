
using System.Text.Json;
using Colibo.Models;
using Colibo.Service.MergedData_Service;

namespace Colibo.Service.Json_Service;

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

      await SerializeAsync(mergedUsers);
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
      var mergedUsers = await DeserializeAsync();

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
      var list = await DeserializeAsync();

      if (list != null)
      {
        list.Add(newUser);
        await SerializeAsync(list);

        logger!.LogInformation($"New user added to file successfully: {newUser.Id}, {list.Count}");
      }
      else
      {
        logger!.LogError("Deserialization of JSON content failed.");
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
      var list = await DeserializeAsync();

      if (list != null)
      {
        var toRemove = list.FirstOrDefault(u => u.Id!.Equals(id));
        if (toRemove != null)
        {
          list.Remove(toRemove);
          await SerializeAsync(list);

          logger!.LogInformation($" Id deleted successfully: {id}");
        }
        else
        {
          logger!.LogError($" Id: {id} not found.");
        }
      }
      else
      {
        logger!.LogError("Deserialization of JSON content failed.");
      }
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error deleting user from JSON file: {ex.Message}");
      throw;
    }
  }

  public async Task UpdateAsync(MergedUsers updateUser)
  {
    try
    {
      var list = await DeserializeAsync();

      if (list != null)
      {
        var toUpdate = list.FirstOrDefault(u => u.Id!.Equals(updateUser.Id));
        if (toUpdate != null)
        {
          list.Remove(toUpdate);
          list.Add(updateUser);
          await SerializeAsync(list);

          logger!.LogInformation($"Updated user with Id: {updateUser.Id}");
        }
        else
        {
          logger!.LogError($"Id: {updateUser.Id} not found.");
        }
      }
      else
      {
        logger!.LogError("Deserialization of JSON content failed.");
      }
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error deleting user from JSON file: {ex.Message}");
      throw;
    }
  }

  public async Task<MergedUsers> GetByIdAsync(string id)
  {
    try
    {
      var list = await DeserializeAsync();

      if (list == null || list.Count == 0)
      {
        logger.LogError("List is empty.");
        return null!;
      }

      var findUser = list.FirstOrDefault(u => u.Id != null && u.Id.Equals(id));

      if (findUser != null)
      {
        return findUser;
      }
      else
      {
        logger.LogWarning($"User with id: {id} not found.");
        return null!;
      }
    }
    catch (Exception ex)
    {
      logger.LogError($"Error finding the user from JSON: {ex.Message}");
      throw;
    }
  }

  private async Task SerializeAsync(List<MergedUsers> mergedUsers)
  {
    var json = JsonSerializer.Serialize(mergedUsers, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(path, json);
  }

  private async Task<List<MergedUsers>?> DeserializeAsync()
  {
    var content = await File.ReadAllTextAsync(path);
    var mergedUsers = JsonSerializer.Deserialize<List<MergedUsers>>(content);
    return mergedUsers;
  }
}