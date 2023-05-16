using System.Text.Json.Nodes;

namespace WebApp.V3.Models;

public class WorkflowDefinitionRequest
{
    public string CorrelationId { get; set; }
    public JsonObject Input { get; set; }
}