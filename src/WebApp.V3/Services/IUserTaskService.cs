using Newtonsoft.Json.Linq;

namespace WebApp.V3.Services;

public interface IUserTaskService
{
    Task<UserTaskViewModel> GetUserTasksFor(string workflowInstanceId);

    Task MarkAsCompleteDispatched(string workflowInstanceId, string activityId, bool goToPrevious,
        string signal, JToken signalData);
}