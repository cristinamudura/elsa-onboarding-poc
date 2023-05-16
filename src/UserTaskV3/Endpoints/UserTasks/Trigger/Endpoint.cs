using System.Text.Json.Serialization;
using Elsa.Abstractions;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Serialization.Converters;
using JetBrains.Annotations;
using UserTaskV3.Contracts;

namespace UserTaskV3.Endpoints.UserTasks.Trigger;

[PublicAPI]
public class UserTask : ElsaEndpoint<UserTaskRequest>
{
    private readonly IUserTaskPublisher _userTaskPublisher;

    public UserTask(IUserTaskPublisher userTaskPublisher)
    {
        _userTaskPublisher = userTaskPublisher;
    }
    
    public override void Configure()
    {
        Post("/user-tasks/{eventName}/trigger");
        ConfigurePermissions("trigger:user-task");
    }

    public override async Task HandleAsync(UserTaskRequest request, CancellationToken cancellationToken)
    {
        var input = (IDictionary<string, object>?)request.Input;
        var eventName = request.EventName;
        var correlationId = request.CorrelationId;
        var workflowInstanceId = request.WorkflowInstanceId;
        var workflowExecutionMode = request.WorkflowExecutionMode;

        if(workflowExecutionMode == WorkflowExecutionMode.Asynchronous)
            await _userTaskPublisher.DispatchAsync(eventName, correlationId, workflowInstanceId, input, cancellationToken);
        else
            await _userTaskPublisher.PublishAsync(eventName, correlationId, workflowInstanceId, input, cancellationToken);

        if (!HttpContext.Response.HasStarted) 
            await SendOkAsync(cancellationToken);
    }
}


public class UserTaskRequest
{
    public string EventName { get; set; } = default!;
    public string? CorrelationId { get; set; }
    
    [JsonConverter(typeof(ExpandoObjectConverterFactory))]
    public object? Input { get; set; }

    public WorkflowExecutionMode WorkflowExecutionMode { get; set; } = WorkflowExecutionMode.Asynchronous;
    public string? WorkflowInstanceId { get; set; }
}