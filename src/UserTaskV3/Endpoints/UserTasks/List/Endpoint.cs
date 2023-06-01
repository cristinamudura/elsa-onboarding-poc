using Elsa.Abstractions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Management.Filters;
using Elsa.Workflows.Runtime.Contracts;
using JetBrains.Annotations;
using UserTaskV3.Activities;

namespace UserTaskV3.Endpoints.UserTasks.List;

[PublicAPI]
internal class Endpoint : ElsaEndpointWithoutRequest<IEnumerable<Response>>
{
    private readonly IBookmarkStore _bookmarkStore;
    private readonly IWorkflowInstanceStore _workflowInstanceStore;
    private readonly ITriggerStore _triggerStore;


    public Endpoint(IBookmarkStore bookmarkStore, IWorkflowInstanceStore workflowInstanceStore,
        ITriggerStore triggerStore)
    {
        _bookmarkStore = bookmarkStore;
        _workflowInstanceStore = workflowInstanceStore;
        _triggerStore = triggerStore;
    }

    public override void Configure()
    {
        Get("/user-tasks/instances");
        ConfigurePermissions("read:user-tasks");
    }

    public override async Task<IEnumerable<Response>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var activityType = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();
        var bookmarkFilter = new BookmarkFilter { ActivityTypeName = activityType };

        var bookmarks = await _bookmarkStore.FindManyAsync(bookmarkFilter, cancellationToken);

        var workflowInstances = bookmarks.Select(b => b.WorkflowInstanceId);

        var response = await _workflowInstanceStore.FindManyAsync(new WorkflowInstanceFilter()
        {
            Ids = workflowInstances.ToList()
        }, cancellationToken);

        
        return response.Select( i => new Response
        {
            CorrelationId = i.CorrelationId,
            CreatedAt = i.CreatedAt.DateTime,
            LastExecutedAt = i.LastExecutedAt.Value.DateTime,
            Name = i.Name,
            WorkflowInstanceId = i.Id
        });
    }
}