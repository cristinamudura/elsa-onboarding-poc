using System.Text.Json.Serialization;

namespace WebApp.V3.Models;

public class WorkflowDefinitionResponse
{
    [JsonPropertyName("workflowInstanceId")]
    public string WorkflowInstanceId { get; set; }
    
    [JsonPropertyName("activityId")]
    public string ActivityId { get; set; }
}