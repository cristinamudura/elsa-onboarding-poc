// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using System.Text.Json.Nodes;
// using WebApp.V3.Models;
//
// namespace WebApp.V3.Services;
//
// public interface IUserTaskService
// {
//     Task<object> GetUserTasksFor(string workflowInstanceId)
// }
//
// public class UserTaskService : IUserTaskService
// {
//     private readonly IHttpClientFactory _httpClientFactory;
//     private readonly IAuthorizationService _authorizationService;
//     private readonly IWorkflowInstanceService _workflowInstanceService;
//
//     public UserTaskService(IHttpClientFactory httpClientFactory, IAuthorizationService authorizationService,
//         IWorkflowInstanceService workflowInstanceService)
//     {
//         _httpClientFactory = httpClientFactory;
//         _authorizationService = authorizationService;
//         _workflowInstanceService = workflowInstanceService;
//     }
//
//     public async Task<object> GetUserTasksFor(string workflowInstanceId)
//     {
//         var workflowInstance = _workflowInstanceService.GetWorkflowInstance(workflowInstanceId);
//         
//         var bookmarks = workflowInstanceId
//         
//         
//         var httpClient = _httpClientFactory.CreateClient("UserTaskService");
//         var items = await httpClient.GetFromJsonAsync<WorkflowInstanceUserTaskViewModel[]>($"/v1/usertask-signals?workflowinstanceId={workflowinstanceId}");
//         return items?.FirstOrDefault();
//     }
// }