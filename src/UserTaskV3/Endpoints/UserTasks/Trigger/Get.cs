using System.Text.Json.Serialization;
using Azure;
using Elsa.Abstractions;
using Elsa.Extensions;
using Elsa.Workflows.Core.Helpers;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Serialization.Converters;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Management.Entities;
using Elsa.Workflows.Management.Filters;
using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using UserTaskV3.Contracts;

namespace UserTaskV3.Endpoints.UserTasks.Trigger;

[PublicAPI]
public class Get : ElsaEndpointWithoutRequest<WorkflowInstance>
{
    private readonly IBookmarkStore _bookmarkStore;
    private readonly IWorkflowInstanceStore _workflowInstanceStore;


    public Get(IBookmarkStore bookmarkStore, IWorkflowInstanceStore workflowInstanceStore)
    {
        _bookmarkStore = bookmarkStore;
        _workflowInstanceStore = workflowInstanceStore;
    }

    public override void Configure()
    {
        Get("/user-tasks");
        ConfigurePermissions("read:*", "read:storage-drivers");
    }

    public override async Task<WorkflowInstance> ExecuteAsync(CancellationToken cancellationToken)
    {
        var activityType = ActivityTypeNameHelper.GenerateTypeName<Activities.UserTask>();
        var bookmarkFilter = new BookmarkFilter { ActivityTypeName = activityType };

        var bookmarks = await _bookmarkStore.FindManyAsync(bookmarkFilter, cancellationToken);
        var response = bookmarks.ToList();
        
        
        var instanceFilter = new WorkflowInstanceFilter { Id = response.First().WorkflowInstanceId };
        var workflowInstance = await _workflowInstanceStore.FindAsync(instanceFilter, cancellationToken);
        
        return workflowInstance;
    }
}
