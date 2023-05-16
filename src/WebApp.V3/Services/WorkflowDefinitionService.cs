using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public class WorkflowDefinitionService
{
    private readonly IHttpClientFactory httpClientFactory;

    public WorkflowDefinitionService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<WorkflowDefinitionResponse> StartWorkflowDefinition(string workflowDefinitionId, string? correlationId = null, JsonObject? input= null)
    {
        var httpClient = httpClientFactory.CreateClient("WorkflowDefinitionServiceClient");
        
        var data = new WorkflowDefinitionRequest
        {
            CorrelationId = correlationId,
            Input = input ?? new JsonObject()
        };

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"v1/workflows/{workflowDefinitionId}/dispatch", content);

        return await response.Content.ReadFromJsonAsync<WorkflowDefinitionResponse>();
    }
}