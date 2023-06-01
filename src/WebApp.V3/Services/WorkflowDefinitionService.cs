using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IAuthorizationService authorizationService;

    public WorkflowDefinitionService(IHttpClientFactory httpClientFactory, IAuthorizationService authorizationService)
    {
        this.httpClientFactory = httpClientFactory;
        this.authorizationService = authorizationService;
    }

    public async Task<WorkflowDefinitionResponse> StartWorkflowDefinition(string workflowDefinitionId, string? correlationId = null, JsonObject? input= null)
    {
        var httpClient = httpClientFactory.CreateClient("WorkflowDefinitionServiceClient");
        var data = new WorkflowDefinitionRequest
        {
            CorrelationId = correlationId,
            Input = input ?? new JsonObject()
        };

        var authenticationResponse = await authorizationService.GetAccessToken();
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);

        
        var response = await httpClient.PostAsync($"elsa/api/workflow-definitions/{workflowDefinitionId}/dispatch", content);

        return await response.Content.ReadFromJsonAsync<WorkflowDefinitionResponse>();
    }
}