using Elsa.Abstractions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Management.Filters;
using Elsa.Workflows.Runtime.Contracts;
using JetBrains.Annotations;
using UserTaskV3.Activities;
using UserTaskV3.Bookmarks;

namespace UserTaskV3.Endpoints.UserTasks.Get;

[PublicAPI]
internal class Endpoint : ElsaEndpoint<Request, Response?>
{
    private readonly IWorkflowInstanceStore _workflowInstanceStore;

    public Endpoint(IBookmarkStore bookmarkStore, IWorkflowInstanceStore workflowInstanceStore,
        ITriggerStore triggerStore)
    {
        _workflowInstanceStore = workflowInstanceStore;
    }

    public override void Configure()
    {
        Get("/user-tasks/instances/{workflowInstanceId}");
        ConfigurePermissions("read:user-tasks");
    }

    public override async Task<Response?> ExecuteAsync(Request userTaskRequest,
        CancellationToken cancellationToken)
    {
        var instanceFilter = new WorkflowInstanceFilter {Id = userTaskRequest.WorkflowInstanceId};
        var workflowInstance = await _workflowInstanceStore.FindAsync(instanceFilter, cancellationToken);

        if (workflowInstance == null)
        {
            return null;
        }
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();
        var customUserTask = workflowInstance.WorkflowState.Bookmarks.FirstOrDefault(t => t.Name == activityTypeName);
        if (customUserTask != null)
        {
            var userTaskId = customUserTask.ActivityInstanceId;
            var activityInfo = workflowInstance.WorkflowState.ActivityExecutionContexts.Single(c => c.Id.Equals(userTaskId));

            var allowPrevious = activityInfo.ActivityState["AllowPrevious"] ?? false;

            var payload = customUserTask.Payload as IncomingUserTaskBookmarkPayload;
            return new Response
            {
                AllowPrevious = (bool)allowPrevious,
                UserTaskActivityInstanceId = userTaskId,
                UIDefinition = activityInfo.ActivityState["UIDefinition"].ToString(),
                Metadata = workflowInstance.WorkflowState.Properties,
                ActivityId = payload.ActivityId
            };
        }

        return null;
    }
}