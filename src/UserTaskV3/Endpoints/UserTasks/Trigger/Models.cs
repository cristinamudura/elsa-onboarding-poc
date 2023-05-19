using System.Text.Json.Serialization;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Serialization.Converters;

namespace UserTaskV3.Endpoints.UserTasks.Trigger;

internal class UserTaskRequest
{
    public string EventName { get; set; } = default!;
    public string? CorrelationId { get; set; }
    
    [JsonConverter(typeof(ExpandoObjectConverterFactory))]
    public object? Input { get; set; }

    public WorkflowExecutionMode WorkflowExecutionMode { get; set; } = WorkflowExecutionMode.Asynchronous;
    public string? WorkflowInstanceId { get; set; }
    
    public string? ActivityId { get; set; }

}