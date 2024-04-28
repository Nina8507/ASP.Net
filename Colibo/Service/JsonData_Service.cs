
using System.Text.Json;
using Colibo.Models;

namespace Colibo.Service;

public class JsonData_Service : IJsonData_Service
{
  private readonly ILogger<JsonData_Service>? logger;

  public JsonData_Service(ILogger<JsonData_Service> logger)
  {
    this.logger = logger;
  }

  public async Task SerializeToJsonAsync(string path, List<MergedUsers> mergedUsers)
  {
    try
    {
      logger!.LogInformation($"List of Users from service to JSON: {mergedUsers.Count}");
      string json = JsonSerializer.Serialize(mergedUsers, new JsonSerializerOptions { WriteIndented = true });
      await File.WriteAllTextAsync(path, json);
      logger!.LogInformation($"List of Users from service to JSON after serialization: {mergedUsers.Count}");
      logger!.LogInformation($"Json file created successfully: {path}");
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error creating JSON file: {ex.Message}");
      throw;
    }
  }
  public async Task SaveNewUserAsync(string path, MergedUsers newUser)
  {
    try
    {
      var content = await File.ReadAllTextAsync(path);
      var list = JsonSerializer.Deserialize<List<MergedUsers>>(content);

       if (list != null)
        {
          list.Add(newUser);
          await SerializeToJsonAsync(path, list);

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
}