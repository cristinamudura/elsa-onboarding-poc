using System.Text;
using Elsa.OnBoardingProcess.PoC.Requests;
using Elsa.OnBoardingProcess.PoC.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.OnBoardingProcess.PoC.Services;

public class UserTaskService
{
    private readonly IHttpClientFactory httpClientFactory;

    public UserTaskService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<UserTaskViewModel[]> GetWorkflowsForSignal(string signal)
    {
        var httpClient = httpClientFactory.CreateClient("UserTaskService");
        return await httpClient.GetFromJsonAsync<UserTaskViewModel[]>($"/v1/usertask-signals/{signal}");
    }


    public async Task<WorkflowInstanceUserTaskViewModel[]> GetWorkflowsWaitingOnUserTask()
    {
        var httpClient = httpClientFactory.CreateClient("UserTaskService");
        return await httpClient.GetFromJsonAsync<WorkflowInstanceUserTaskViewModel[]>($"/v1/usertask-signals");
    }

    public async Task<WorkflowInstanceUserTaskViewModel> GetUserTasksFor(string workflowinstanceId)
    {
        var httpClient = httpClientFactory.CreateClient("UserTaskService");
        var items = await httpClient.GetFromJsonAsync<WorkflowInstanceUserTaskViewModel[]>($"/v1/usertask-signals?workflowinstanceId={workflowinstanceId}");
        return items?.FirstOrDefault();
    }

    public async Task MarkAsCompleteDispatched(string workflowInstanceId, string signal, JToken signalData)
    {
        var httpClient = httpClientFactory.CreateClient("UserTaskService");
        var data = new MarkAsCompletedRequest
        {
            WorkflowInstanceId = workflowInstanceId,
            Input = signalData == null ? JValue.CreateNull() : signalData
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        await httpClient.PostAsync($"/v1/usertask-signals/{signal}/dispatch", content);
    }
}