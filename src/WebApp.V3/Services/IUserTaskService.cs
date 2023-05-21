using Newtonsoft.Json.Linq;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public interface IUserTaskService
{
    Task<UserTaskViewModel> GetUserTasksFor(string workflowInstanceId);

    Task MarkAsCompleteDispatched(string workflowInstanceId, string activityId, bool goToPrevious,
        string signal, JToken signalData);

    Task<List<WorkflowInstanceModel>> GetWorkflowsWaitingOnUserTask();
}