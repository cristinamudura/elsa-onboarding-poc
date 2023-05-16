using Microsoft.AspNetCore.SignalR;

namespace UserTaskV3.Hubs;

public class WorkflowInstanceInfoHub : Hub<IWorkflowInstanceInfoHub>
{
    public async Task JoinWorkflowInstanceGroup(string workflowInstanceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, workflowInstanceId);
    }

    public async Task LeaveWorkflowInstanceGroup(string workflowInstanceId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, workflowInstanceId);
    }

}