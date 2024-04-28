using Colibo.Models;

namespace Colibo.Service.MergedData_Service;

public interface IMergedData_Service
{
  Task<List<MergedUsers>> GetAllAsync();
}