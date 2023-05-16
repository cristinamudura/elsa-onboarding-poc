using Elsa.Extensions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Models.Requests;
using UserTaskV3.Activities;
using UserTaskV3.Bookmarks;

namespace UserTaskV3.Contracts;

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
    public async Task PublishAsync(string eventName, string? correlationId = default, string? workflowInstanceId = default, IDictionary<string, object>? input = default, CancellationToken cancellationToken = default)
    {
        var eventBookmark = new IncomingUserTaskBookmarkPayload(eventName);
        var options = new TriggerWorkflowsRuntimeOptions(correlationId, workflowInstanceId, input);
        await _workflowRuntime.TriggerWorkflowsAsync<UserTask>(eventBookmark, options, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DispatchAsync(string eventName, string? correlationId = default, string? workflowInstanceId = default, IDictionary<string, object>? input = default, CancellationToken cancellationToken = default)
    {
        var eventBookmark = new EventBookmarkPayload(eventName);
        var activityTypeName = ActivityTypeNameHelper.GenerateTypeName<UserTask>();
        var request = new DispatchTriggerWorkflowsRequest(activityTypeName, eventBookmark, correlationId, workflowInstanceId, input);
        await _workflowDispatcher.DispatchAsync(request, cancellationToken);
    }
}