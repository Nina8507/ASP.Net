using Colibo.Models;

namespace Colibo.Service;

public interface IMergedData_Service
{
  Task<List<MergedUsers>> GetAllAsync();
  Task AddAync(MergedUsers newUser);
  Task UpdateAsync(string id);
  Task DeleteAsync(string id);
  Task<MergedUsers> GetByIdAsync(string id);
}