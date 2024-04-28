using System.Net.Http.Headers;
using System.Text.Json;
using Colibo.Models;
using Microsoft.Identity.Client;

namespace Colibo.Data.AZURE_Context;

public class AzureAD_Context(ILogger<AzureAD_Context> logger) : IContext_Azure
{
  private readonly string clientId = "1d071267-e9b0-449c-bf44-fe278a923929";
  private readonly string clientSecret = "Kw_c09wp-u6PJ5siFtSu2Vu-..5_I33W~a";
  private readonly string authority = "https://login.microsoftonline.com/88592590-0df3-4d4f-8f2b-c86731dc0c44";
  private readonly string[] scopes = ["User.Read.All"];
  private readonly ILogger<AzureAD_Context> logger = logger;

  public async Task<UserData> Initialize_Azure_Async()
  {
    UserData? userData = new();

    var app = ConfidentialClientApplicationBuilder.Create(clientId)
          .WithClientSecret(clientSecret)
          .WithAuthority(new Uri(authority))
          .Build();

    var tokenResponse = await app.AcquireTokenForClient([".default"]).ExecuteAsync();

    using var client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

    var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users");

    if (response.IsSuccessStatusCode)
    {
      var content = await response.Content.ReadAsStringAsync();
      logger.LogInformation($"Response content from Azure is size: {content.Count()}");
      userData = JsonSerializer.Deserialize<UserData>(content, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });
    }
    else
    {
      logger.LogInformation($"Failed to read from Azure: {response.StatusCode}");
    }

    return userData!;
  }
}

