using System.Text.Json.Serialization;
using Elsa.Extensions;
using Elsa.Workflows.Core.Activities.Flowchart.Attributes;
using Elsa.Workflows.Core.Attributes;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Models;
using JetBrains.Annotations;
using UserTaskV3.Bookmarks;

namespace UserTaskV3.Activities;

[Activity(
    "UserOnBoard",
    "UserOnBoard")]
[PublicAPI]
[FlowNode("Next", "Previous")]
public class UserTask : Trigger<object?>
{
    [JsonConstructor]
    public UserTask()
    {
    }

    [Input(Description = "The UI Definition as type")]
    public Input<string> UIDefinition { get; set; } = default!;

    [Input(Description = "The name of the event to listen for.")]
    public Input<string>? EventName { get; set; } = default!;

    [Input(Description = "Allow previous")]
    public Input<bool>? AllowPrevious { get; set; }

    [Input(Description = "The user input")]
    public Input<UserTaskInput> UserTaskInput { get; set; } = default!;
    
    [Input(Description = "The user task name that identifies it.")]
    public Input<string> UserTaskName { get; set; } = default!;

    public Output<object?> UserTaskData { get; set; } = default!;

    protected override object GetTriggerPayload(TriggerIndexingContext context)
    {
        var eventName = EventName.Get(context.ExpressionExecutionContext);
        return new IncomingUserTaskBookmarkPayload(eventName);
    }

    protected override async ValueTask OnSignalReceivedAsync(object signal, SignalContext context)
    {
        var executionContext = context.SenderActivityExecutionContext;
        var eventName = executionContext.Get(EventName)!;

        var userTaskName = executionContext.Get(UserTaskName);
        var allowPrevious = executionContext.Get(AllowPrevious);
        var input = executionContext.Input;
        
        AddOrUpdateMetadata(executionContext, input);

        // if (allowPrevious && input.GoToPrevious)
        // {
        //     await context.CompleteActivityWithOutcomesAsync("Previous");
        // }
        //
        // context.Set(UserTaskData, input.Data);
        // context.SetVariable(userTaskName, input.Data);
        await executionContext.CompleteActivityWithOutcomesAsync("Next");
    }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var eventName = context.Get(EventName)!;
        if (!context.IsTriggerOfWorkflow())
        {
            context.CreateBookmark(new IncomingUserTaskBookmarkPayload(eventName));
            return;
        }

        var userTaskName = context.Get(UserTaskName);
        var allowPrevious = context.Get(AllowPrevious);
        var input = context.Get(UserTaskInput);
        AddOrUpdateMetadata(context, input.Data);

        if (allowPrevious && input.GoToPrevious)
        {
            await context.CompleteActivityWithOutcomesAsync("Previous");
        }

        context.Set(UserTaskData, input.Data);
        context.SetVariable(userTaskName, input.Data);
        //await context.CompleteActivityWithOutcomesAsync("Next");
    }

    private void AddOrUpdateMetadata(ActivityExecutionContext context, object data)
    {
        if (context.WorkflowExecutionContext.TransientProperties.ContainsKey(context.Activity.Id))
        {
            context.WorkflowExecutionContext.TransientProperties[context.Activity.Id] = data;
            return;
        }

        context.WorkflowExecutionContext.TransientProperties.Add(context.Activity.Id, data);
    }
}

public class UserTaskInput
{
    public object? Data { get; set; } = default!;
    public bool GoToPrevious { get; set; }
}