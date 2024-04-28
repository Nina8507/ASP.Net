using Colibo.Models;

namespace Colibo.Data.AZURE_Context
{
  public interface IContext_Azure
  {
    Task<UserData> Initialize_Azure_Async();
  }
}