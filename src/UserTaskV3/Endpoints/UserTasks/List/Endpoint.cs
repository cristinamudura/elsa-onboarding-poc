using Elsa.Abstractions;
using Elsa.Extensions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Management.Entities;
using Elsa.Workflows.Management.Filters;
using Elsa.Workflows.Runtime.Contracts;
using JetBrains.Annotations;
using UserTaskV3.Bookmarks;

namespace UserTaskV3.Endpoints.UserTasks.List;

[PublicAPI]
public class Endpoint : ElsaEndpointWithoutRequest<WorkflowInstance>
{
    private readonly IBookmarkStore _bookmarkStore;
    private readonly IWorkflowInstanceStore _workflowInstanceStore;
    private readonly ITriggerStore _triggerStore;


    public Endpoint(IBookmarkStore bookmarkStore, IWorkflowInstanceStore workflowInstanceStore, ITriggerStore triggerStore)
    {
        _bookmarkStore = bookmarkStore;
        _workflowInstanceStore = workflowInstanceStore;
        _triggerStore = triggerStore;
    }

    public override void Configure()
    {
        Get("/user-tasks");
        ConfigurePermissions("read:*", "read:storage-drivers");
    }

    public override async Task<WorkflowInstance> ExecuteAsync(CancellationToken cancellationToken)
    {
        var activityType = ActivityTypeNameHelper.GenerateTypeName<Activities.UserTask>();
        
        var triggerFilter = new TriggerFilter { Name = activityType};
        var triggers = (await _triggerStore.FindManyAsync(triggerFilter, cancellationToken))
            .Select(x => x.GetPayload<IncomingUserTaskBookmarkPayload>()).ToList();

        var bookmarkFilter = new BookmarkFilter { ActivityTypeName = activityType };

        var bookmarks = (await _bookmarkStore.FindManyAsync(bookmarkFilter, cancellationToken)).Select(x => x.GetPayload<IncomingUserTaskBookmarkPayload>()).ToList();

        var response = bookmarks.ToList();
        
        var payloads = triggers.Concat(bookmarks).ToList();

        var workflowInstanceId = Query<string>("workflowInstanceId");
        if (!string.IsNullOrWhiteSpace(workflowInstanceId))
        {
            var instanceFilter = new WorkflowInstanceFilter { Id = workflowInstanceId };
            var workflowInstance = await _workflowInstanceStore.FindAsync(instanceFilter, cancellationToken);
        }
        
        return null;
    }
}
