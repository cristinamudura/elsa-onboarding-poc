using System.Text.Json.Serialization;
using Elsa.Extensions;
using Elsa.Workflows.Core.Activities.Flowchart.Attributes;
using Elsa.Workflows.Core.Attributes;
using Elsa.Workflows.Core.Models;
using JetBrains.Annotations;
using UserTaskV3.Bookmarks;
using UserTaskV3.Models;

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
    public Input<string>? EventName { get; set; } = new(string.Empty);

    [Input(Description = "Allow previous")]
    public Input<bool>? AllowPrevious { get; set; } = new(false);
     
    [Input(Description = "The user task name that identifies it.")]
    public Input<string> UserTaskName { get; set; } = default!;

    public Output<object?> UserTaskData { get; set; } = default!;
  
    protected override object GetTriggerPayload(TriggerIndexingContext context)
    {
        var eventName = EventName.Get(context.ExpressionExecutionContext);
        return new IncomingUserTaskBookmarkPayload(eventName, Id);
    }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var eventName = context.Get(EventName)!;
        if (!context.IsTriggerOfWorkflow())
        {
            context.CreateBookmark(new IncomingUserTaskBookmarkPayload(eventName, Id), OnResume);
        }
    }

    private async ValueTask OnResume(ActivityExecutionContext context)
    {
        var executionContext = context;
 
        var allowPrevious = executionContext.Get(AllowPrevious);
        
        var userTaskInput = executionContext.GetInput<UserTaskSignalInput>();
        
        context.Set(UserTaskData, userTaskInput.Input);

        AddOrUpdateMetadata(executionContext, userTaskInput.Input);

        if (allowPrevious && userTaskInput.GoToPrevious)
        {
            await context.CompleteActivityWithOutcomesAsync("Previous");
            return;
        }
        await executionContext.CompleteActivityWithOutcomesAsync("Next");
    }

    private void AddOrUpdateMetadata(ActivityExecutionContext context, object data)
    {
        if (context.WorkflowExecutionContext.Properties.ContainsKey(context.Activity.Id))
        {
            context.WorkflowExecutionContext.Properties[context.Activity.Id] = data;
            return;
        }

        context.WorkflowExecutionContext.Properties.Add(context.Activity.Id, data);
    }
}