using Elsa.Models;
using UserTask.AddOns.Endpoints.Models;

namespace UserTask.AddOns.Extensions
{
    public static class ViewModelExtensions
    {
        internal static List<WorkflowInstanceUserTaskViewModel> ConvertToWorkflowInstanceUsertaskViewModels(this IEnumerable<WorkflowInstance> source, ServerContext serverContext, IEnumerable<Bookmark> bookmarks)
        {
            var result = new List<WorkflowInstanceUserTaskViewModel>();
            source.ToList().ForEach(x =>
            result.Add(new WorkflowInstanceUserTaskViewModel
            {
                WorkflowInstanceId = x.Id,
                WorkflowName = x.Name ?? "not set",
                LastExecuted = x.LastExecutedActivityId,
                State = x.WorkflowStatus.ToString(),
                DefinitionId = x.DefinitionId,
                UserTasks = x.ConvertToUserTaskViewModels(serverContext),
                Metadata = x.Metadata,
                CurrentActivityId = x.BlockingActivities?.FirstOrDefault()?.ActivityId ?? x?.CurrentActivity?.ActivityId ?? string.Empty,
                Correlation = x.CorrelationId,
                StartDate = x.CreatedAt.ToDateTimeUtc()
            })
            );
            return result;
        }

        internal static List<UserTaskViewModel> ConvertToUsertaskViewModels(this IEnumerable<WorkflowInstance> source, ServerContext serverContext)
        {
            var result = new List<UserTaskViewModel>();
            source.ToList().ForEach(i => result.AddRange(i.ConvertToUserTaskViewModels(serverContext)));
            return result;
        }

        internal static List<UserTaskViewModel> ConvertToUserTaskViewModels(this WorkflowInstance i, ServerContext serverContext)
        {
            var result = new List<UserTaskViewModel>();
            i.BlockingActivities.ToList().ForEach(b => result.Add(b.ConvertToViewModel(i, serverContext)));
            return result;
        }
        internal static UserTaskViewModel ConvertToViewModel(this BlockingActivity b, WorkflowInstance i, ServerContext serverContext)
        {
            var result = new UserTaskViewModel();
            var data = i.ActivityData[b.ActivityId];
            result.WorkflowInstanceId = i.Id;
            result.Signal = data["Signal"].ToString();
            result.TaskDescription = data["TaskDescription"]?.ToString();
            result.TaskName = data["TaskName"]?.ToString();
            result.TaskTitle = data["TaskTitle"]?.ToString();
            result.UIDefinition = data["UIDefinition"]?.ToString();
            result.EngineId = serverContext.EngineId;
            result.AllowPrevious = Convert.ToBoolean(data["AllowPrevious"]);
            return result;
        }
    }
}
