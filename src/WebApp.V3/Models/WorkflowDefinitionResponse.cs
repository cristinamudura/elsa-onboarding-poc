using System.Text.Json.Serialization;

namespace WebApp.V3.Models;

public class WorkflowDefinitionResponse
{
    [JsonPropertyName("workflowInstanceId")]
    public string WorkflowInstanceId { get; set; }
    
    [JsonPropertyName("activityId")]
    public string ActivityId { get; set; }
}

public class WorkflowDefinitionExecuteResponse
{
    [JsonPropertyName("workflowState")]
    public WorkflowState WorkflowState { get; set; }
}

public class WorkflowState
{
    [JsonPropertyName("id")]
    public string WorkflowInstanceId { get; set; }
}