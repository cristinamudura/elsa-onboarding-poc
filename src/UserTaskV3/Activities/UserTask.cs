using System.Text.Json.Serialization;
using Elsa.Extensions;
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
public class UserTask : Trigger<object?>
{
    [JsonConstructor]
    public UserTask()
    {
    }
    

    [Input] public Input<string> UIDefinition { get; set; } = default!;


    [Input(Description = "The name of the event to listen for.")]
    public Input<string>? EventName { get; set; } = default!;
    
    
    protected override object GetTriggerPayload(TriggerIndexingContext context)
    {
        var eventName = EventName.Get(context.ExpressionExecutionContext);
        return new IncomingUserTaskBookmarkPayload(eventName);
    }

    protected override async ValueTask OnSignalReceivedAsync(object signal, SignalContext context)
    {
        var executionContext = context.SenderActivityExecutionContext;
       var eventName = executionContext.Get(EventName)!;
        // if (!executionContext.IsTriggerOfWorkflow())
        // {
        //     executionContext.CreateBookmark(new IncomingUserTaskBookmarkPayload(eventName));
        //     return;
        // }
        
        //await executionContext.CompleteActivityAsync();
    }

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var eventName = context.Get(EventName)!;
        if (!context.IsTriggerOfWorkflow())
        {
            context.CreateBookmark(new IncomingUserTaskBookmarkPayload(eventName));
            return;
        }
        
        await context.CompleteActivityAsync();

    }
}