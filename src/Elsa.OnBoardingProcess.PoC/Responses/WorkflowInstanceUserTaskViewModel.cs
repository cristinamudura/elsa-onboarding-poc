using Newtonsoft.Json;

namespace Elsa.OnBoardingProcess.PoC.Responses;

public class WorkflowInstanceUserTaskViewModel
{
    [JsonProperty("workflowInstanceId")]
    public string WorkflowInstanceId { get; set; } = string.Empty;

    [JsonProperty("lastExecuted")]
    public string? LastExecuted { get; set; }

    [JsonProperty("state")]
    public string State { get; set; } = string.Empty;

    [JsonProperty("definitionId")]
    public string DefinitionId { get; set; } = string.Empty;

    [JsonProperty("workflowName")]
    public string WorkflowName { get; set; } = string.Empty;

    [JsonProperty("userTasks")]
    public List<UserTaskViewModel> UserTasks { get; set; } = new();
    
    [JsonProperty("Metadata")]
    public IDictionary<string, object?> Metadata { get; set; } = new Dictionary<string, object?>();
    
    [JsonProperty("currentActivityId")]
    public string CurrentActivityId { get; set; } = string.Empty;
    
    [JsonProperty("correlation")]
    public string Correlation { get; set; } = string.Empty;
    
    [JsonProperty("startDate")]
    public DateTime StartDate { get; set; }
}