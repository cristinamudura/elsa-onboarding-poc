using Newtonsoft.Json;

namespace UserTask.AddOns.Endpoints.Models
{
    internal class WorkflowInstanceUserTaskViewModel
    {
        [JsonProperty("workflowInstanceId")]
        public string WorkflowInstanceId { get; set; } = string.Empty;
        
        [JsonProperty("lastExecuted")]
        internal string? LastExecuted { get;  set; }
        
        [JsonProperty("state")] 
        internal string State { get; set; } = string.Empty;
        
        [JsonProperty("definitionId")] 
        internal string DefinitionId { get;  set; } = string.Empty;
        
        [JsonProperty("workflowName")] 
        internal string WorkflowName { get; set; } = string.Empty;
        
        [JsonProperty("userTasks")] 
        internal List<UserTaskViewModel> UserTasks { get; set; } = new List<UserTaskViewModel>();

        [JsonProperty("metadata")] 
        internal IDictionary<string, object?> Metadata { get; set; } = new Dictionary<string, object?>();
        
        [JsonProperty("currentActivityId")] 
        internal string CurrentActivityId { get;  set; } = string.Empty;
        
        [JsonProperty("correlation")] 
        internal string Correlation { get;  set; } = string.Empty;
        
        [JsonProperty("startDate")] 
        internal DateTime StartDate { get;  set; }
    }
}
