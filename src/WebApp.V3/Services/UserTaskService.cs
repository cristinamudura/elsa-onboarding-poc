using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public class UserTaskService : IUserTaskService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAuthorizationService _authorizationService;

    public UserTaskService(IHttpClientFactory httpClientFactory, IAuthorizationService authorizationService)
    {
        _httpClientFactory = httpClientFactory;
        _authorizationService = authorizationService;
    }

    public async Task<UserTaskViewModel> GetUserTasksFor(string workflowInstanceId)
    {
        var httpClient = _httpClientFactory.CreateClient("UserTaskServiceClient");

        var authenticationResponse = await _authorizationService.GetAccessToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);


        var response = await httpClient.GetAsync($"elsa/api/user-tasks/instances/{workflowInstanceId}");

        return await response.Content.ReadFromJsonAsync<UserTaskViewModel>();
    }

    public async Task MarkAsCompleteDispatched(string workflowInstanceId, string activityId, bool goToPrevious, string signal,
        JToken signalData)
    {
        var httpClient = _httpClientFactory.CreateClient("UserTaskServiceClient");

        var authenticationResponse = await _authorizationService.GetAccessToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);


        var data = new CompleteUserTaskRequest
        {
            WorkflowInstanceId = workflowInstanceId,
            ActivityId = activityId,
            Input = new SignalInput
            {
                UserTaskSignalInput = new UserTaskSignalInput
                {
                    GoToPrevious = goToPrevious,
                    Input = signalData == null ? JValue.CreateNull() : signalData
                }
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        await httpClient.PostAsync($"elsa/api/user-tasks/{signal}/trigger", content);
    }

    public async Task<List<WorkflowInstanceModel>> GetWorkflowsWaitingOnUserTask()
    {
        var httpClient = _httpClientFactory.CreateClient("UserTaskServiceClient");

        var authenticationResponse = await _authorizationService.GetAccessToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);

        return await httpClient.GetFromJsonAsync<List<WorkflowInstanceModel>>("elsa/api/user-tasks/instances");
    }
}

public class UserTaskSignalInput
{
    public bool GoToPrevious { get; set; } = default!;
    public JToken Input { get; set; } = default!;
}

public class CompleteUserTaskRequest
{
    public SignalInput Input { get; set; }

    public int WorkflowExecutionMode { get; set; } = 0;
    public string WorkflowInstanceId { get; set; }
    
    public string ActivityId { get; set; }
}

public class SignalInput
{
    public UserTaskSignalInput UserTaskSignalInput { get; set; }
}