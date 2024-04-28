using Colibo.Models;

namespace Colibo.Service;

public interface IJsonData_Service
{
  Task SerializeToJsonAsync(List<MergedUsers> mergedUsers);
  Task<List<MergedUsers>> ReadJsonFileAsync();
  Task SaveNewUserAsync(MergedUsers mergedUsers);
  Task DeleteUserAsync(string id);
}