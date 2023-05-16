using Elsa.Mediator.Contracts;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Notifications;
using Microsoft.AspNetCore.SignalR;
using UserTaskV3.Hubs;

namespace UserTaskV3.Notifications;

public class WorkflowNotifier : 
    INotificationHandler<ActivityExecuting>,
    INotificationHandler<ActivityExecuted>
{
    private readonly IHubContext<WorkflowInstanceInfoHub, IWorkflowInstanceInfoHub> _hubContext;

    public WorkflowNotifier(IHubContext<WorkflowInstanceInfoHub, IWorkflowInstanceInfoHub> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }
    
    public async Task HandleAsync(ActivityExecuting notification, CancellationToken cancellationToken)
    {
        // find the workflow instance?
        
        await SendNotification(
            notification.ActivityExecutionContext.WorkflowExecutionContext.Id,
            GetInfo(notification.ActivityExecutionContext, "Executing")
        );
    }

    public async Task HandleAsync(ActivityExecuted notification, CancellationToken cancellationToken)
    {
        await SendNotification(
            notification.ActivityExecutionContext.WorkflowExecutionContext.Id,
            GetInfo(notification.ActivityExecutionContext, "Executed")
        );
    }
    
    
    private static WorkflowInstanceInfo GetInfo(ActivityExecutionContext activityNotification, string action)
    {
        //var workflowInstance = activityNotification.WorkflowExecutionContext.WorkflowInstance;

        // return new WorkflowInstanceInfo
        // {
        //     WorkflowInstanceId = workflowInstance.Id,
        //     WorkflowState = workflowInstance.WorkflowStatus.ToString(),
        //     ActivityName = activityNotification.ActivityBlueprint.Name,
        //     ActivityId = activityNotification.Activity.Id,
        //     Action = $"Activity.{action}",
        //     IsUsertask = IsUsertask(activityNotification.Activity.GetType()),
        //     Description = $"{(string.IsNullOrEmpty(activityNotification.ActivityBlueprint.Name) ? activityNotification.Activity.Id : activityNotification.ActivityBlueprint.Name)}"
        // };
        return new WorkflowInstanceInfo();
    }
    
    private async Task SendNotification(string workflowInstanceId, WorkflowInstanceInfo workflowInstanceInfo)
    {
        await _hubContext.Clients.Group(workflowInstanceId).WorkflowInstanceUpdate(workflowInstanceInfo);
    }
}