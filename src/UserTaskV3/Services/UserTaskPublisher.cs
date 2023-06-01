using Elsa.Extensions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Models.Requests;
using UserTaskV3.Activities;
using UserTaskV3.Bookmarks;

namespace UserTaskV3.Services;

public class UserTaskPublisher : IUserTaskPublisher
{
    private readonly IWorkflowRuntime _workflowRuntime;
    private readonly IWorkflowDispatcher _workflowDispatcher;

    /// <summary>
    /// Constructor.
    /// </summary>
    public UserTaskPublisher(IWorkflowRuntime workflowRuntime, IWorkflowDispatcher workflowDispatcher)
    {
        _workflowRuntime = workflowRuntime;
        _workflowDispatcher = workflowDispatcher;
    }
    
    /// <inheritdoc />
    public async Task PublishAsync(string activityId, string? correlationId = default, string? workflowInstanceId = default, IDictionary<string, object>? input = default, CancellationToken cancellationToken = default)
    {
        var eventBookmark = new IncomingUserTaskBookmarkPayload(activityId);
        var options = new TriggerWorkflowsRuntimeOptions(correlationId, workflowInstanceId, input);
        await _workflowRuntime.TriggerWorkflowsAsync<DisplayUIActivity>(eventBookmark, options, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DispatchAsync(string activityId, string? correlationId = default, string? workflowInstanceId = default, IDictionary<string, object>? input = default, CancellationToken cancellationToken = default)
    {
        var eventBookmark = new TriggerWorkflowsRuntimeOptions(activityId);
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<DisplayUIActivity>();
        var request = new DispatchTriggerWorkflowsRequest(activityTypeName, eventBookmark, correlationId, workflowInstanceId, input);
        await _workflowDispatcher.DispatchAsync(request, cancellationToken);
    }
}