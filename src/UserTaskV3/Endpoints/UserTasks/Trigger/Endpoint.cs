using Elsa.Abstractions;
using Elsa.Workflows.Core.Models;
using JetBrains.Annotations;
using UserTaskV3.Services;

namespace UserTaskV3.Endpoints.UserTasks.Trigger;

[PublicAPI]
internal class UserTask : ElsaEndpoint<UserTaskRequest>
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

        if (workflowExecutionMode == WorkflowExecutionMode.Asynchronous)
            await _userTaskPublisher.DispatchAsync(eventName, correlationId, workflowInstanceId, input,
                cancellationToken);
        else
            await _userTaskPublisher.PublishAsync(eventName, request.ActivityId, correlationId, workflowInstanceId, input, cancellationToken);

        if (!HttpContext.Response.HasStarted) 
            await SendOkAsync(cancellationToken);
    }
}
