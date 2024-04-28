
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
      string json = JsonSerializer.Serialize(mergedUsers, new JsonSerializerOptions { WriteIndented = true });
      await File.WriteAllTextAsync(path, json);

      logger!.LogInformation($"Json file created successfully: {path}");
    }
    catch (Exception ex)
    {
      logger!.LogError($"Error creating JSON file: {ex.Message}");
      throw;
    }
  }
  public async Task SaveChangesAsync(string path, List<MergedUsers> mergedUsers)
  {
    string json = JsonSerializer.Serialize(mergedUsers, new JsonSerializerOptions
    {
        WriteIndented = true
    });
    using (StreamWriter outputFile = new(path, false))
    {
      await outputFile.WriteAsync(json);
    }
    logger!.LogInformation($"In the json service size of mergedUsers is: {mergedUsers.Count}");
  }
}