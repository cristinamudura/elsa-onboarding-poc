using Elsa.Mediator.Contracts;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Notifications;
using Microsoft.AspNetCore.SignalR;
using UserTaskV3.Activities;
using UserTaskV3.Hubs;

namespace UserTaskV3.Notifications;

public class WorkflowNotifier : 
    INotificationHandler<ActivityExecuting>,
    INotificationHandler<ActivityExecuted>,
    INotificationHandler<WorkflowExecuted>
{
    private readonly IHubContext<WorkflowInstanceInfoHub, IWorkflowInstanceInfoHub> _hubContext;

    public WorkflowNotifier(IHubContext<WorkflowInstanceInfoHub, IWorkflowInstanceInfoHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }
    
    public async Task HandleAsync(ActivityExecuting notification, CancellationToken cancellationToken)
    {        
        var type = notification.ActivityExecutionContext.Activity.Type;
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();

        if (type == activityTypeName)
        {
            await SendNotification(
                notification.ActivityExecutionContext.WorkflowExecutionContext.Id,
                GetInfo(notification.ActivityExecutionContext, "Executing")
            );
        }
    }

    public async Task HandleAsync(ActivityExecuted notification, CancellationToken cancellationToken)
    {
        var type = notification.ActivityExecutionContext.Activity.Type;
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();

        if (type == activityTypeName)
        {
            await SendNotification(
                notification.ActivityExecutionContext.WorkflowExecutionContext.Id,
                GetInfo(notification.ActivityExecutionContext, "Executed")
            );
        }
    }
    
    private static WorkflowInstanceInfo GetInfo(ActivityExecutionContext activityNotification, string action)
    {
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();
        return new WorkflowInstanceInfo
        {
            ActivityId = activityNotification.Activity.Id,
            ActivityName = activityNotification.ActivityDescriptor.DisplayName,
            WorkflowInstanceId = activityNotification.WorkflowExecutionContext.Id,
            WorkflowState = activityNotification.WorkflowExecutionContext.Status.ToString(),
            Metadata = activityNotification.WorkflowExecutionContext.Properties,
            Action = $"Activity.{action}",
            IsUsertask = activityNotification.Activity.Type == activityTypeName,
            Description = $"{(string.IsNullOrEmpty(activityNotification.ActivityDescriptor.DisplayName) ? activityNotification.Activity.Id : activityNotification.ActivityDescriptor.DisplayName)}",
        };
    }
    
    private async Task SendNotification(string workflowInstanceId, WorkflowInstanceInfo workflowInstanceInfo)
    {
        await _hubContext.Clients.Group(workflowInstanceId).WorkflowInstanceUpdate(workflowInstanceInfo);
    }

    public async Task HandleAsync(WorkflowExecuted notification, CancellationToken cancellationToken)
    {
        await SendNotification(
                notification.WorkflowState.Id,
             new WorkflowInstanceInfo
            {
                WorkflowState = notification.WorkflowState.Status.ToString(),
                IsUsertask = false,
                Description = "Workflow finished"
            }
        );
    }
}