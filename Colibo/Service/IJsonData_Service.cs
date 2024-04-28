using Colibo.Models;

namespace Colibo.Service;

public interface IJsonData_Service
{
  Task SerializeToJsonAsync(string path, List<MergedUsers> mergedUsers);
  Task SaveNewUserAsync(string path, MergedUsers mergedUsers); 
}