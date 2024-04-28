using Colibo.Models;

namespace Colibo.Service;

public interface IMergedData_Service
{
  Task<List<MergedUsers>> GetAllAsync();
}