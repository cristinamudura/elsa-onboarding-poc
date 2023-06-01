using System.Text.Json.Nodes;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public interface IWorkflowDefinitionService
{
    Task<WorkflowDefinitionResponse> StartWorkflowDefinition(string workflowDefinitionId, string? correlationId = null,
        JsonObject? input = null);
}