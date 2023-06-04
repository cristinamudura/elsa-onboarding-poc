using System.Text.Json.Serialization;
using Elsa.Extensions;
using Elsa.Workflows.Core.Activities.Flowchart.Attributes;
using Elsa.Workflows.Core.Attributes;
using Elsa.Workflows.Core.Models;
using JetBrains.Annotations;
using UserTaskV3.Bookmarks;
using UserTaskV3.Models;

namespace UserTaskV3.Activities; 

// Rename to DisplayUIActivity
[Activity("AddOns")]
[PublicAPI]
[FlowNode("Next", "Previous")]
public class DisplayUIActivity : Activity
{
    [JsonConstructor]
    public DisplayUIActivity()
    {
    }

    [Input(Description = "The UI Definition as type")]
    public Input<string> UIDefinition { get; set; } = default!;


    [Input(Description = "Allow previous")]
    public Input<bool>? AllowPrevious { get; set; } = new(false);
     
    public Output<object?> UserTaskData { get; set; } = default!;
  

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        if (!context.IsTriggerOfWorkflow())
        {
            context.CreateBookmark(new IncomingUserTaskBookmarkPayload(Id), OnResume);
        }
    }

    private async ValueTask OnResume(ActivityExecutionContext context)
    {
        var executionContext = context;
 
        var allowPrevious = executionContext.Get(AllowPrevious);
        
        var userTaskInput = executionContext.GetInput<UserTaskSignalInput>();
        
        context.Set(UserTaskData, userTaskInput.Input);
        context.JournalData.Add("UserTaskData", userTaskInput.Input);
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