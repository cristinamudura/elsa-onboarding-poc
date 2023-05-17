using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApp.V3.Models;

namespace WebApp.V3.Services;

public class WorkflowInstanceService : IWorkflowInstanceService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAuthorizationService _authorizationService;

    public WorkflowInstanceService(IHttpClientFactory httpClientFactory, IAuthorizationService authorizationService)
    {
        _httpClientFactory = httpClientFactory;
        _authorizationService = authorizationService;
    }

    public async Task<WorkflowInstanceResponse> GetWorkflowInstance(string workflowInstanceId)
    {
        var httpClient = _httpClientFactory.CreateClient("WorkflowInstanceClient");


        var authenticationResponse = await _authorizationService.GetAccessToken();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticationResponse.AccessToken);


        var response = await httpClient.GetAsync($"elsa/api/workflow-instances/{workflowInstanceId}");

        return await response.Content.ReadFromJsonAsync<WorkflowInstanceResponse>();
    }
}

public interface IWorkflowInstanceService
{
    Task<WorkflowInstanceResponse> GetWorkflowInstance(string workflowInstanceId);
}

public class WorkflowInstanceResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("definitionId")] public string DefinitionId { get; set; }

    [JsonPropertyName("definitionVersionId")]
    public string DefinitionVersionId { get; set; }

    [JsonPropertyName("version")] public long Version { get; set; }

    [JsonPropertyName("workflowState")] public WorkflowState WorkflowState { get; set; }

    [JsonPropertyName("status")] public long Status { get; set; }

    [JsonPropertyName("subStatus")] public long SubStatus { get; set; }

    [JsonPropertyName("correlationId")] public string CorrelationId { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("properties")] public Properties Properties { get; set; }

    [JsonPropertyName("fault")] public Fault Fault { get; set; }

    [JsonPropertyName("createdAt")] public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("lastExecutedAt")] public DateTimeOffset LastExecutedAt { get; set; }

    [JsonPropertyName("finishedAt")] public DateTimeOffset FinishedAt { get; set; }

    [JsonPropertyName("cancelledAt")] public DateTimeOffset CancelledAt { get; set; }

    [JsonPropertyName("faultedAt")] public DateTimeOffset FaultedAt { get; set; }
}

public partial class Fault
{
    [JsonPropertyName("exception")] public ExceptionClass Exception { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("faultedActivityId")]
    public string FaultedActivityId { get; set; }
}

public partial class ExceptionClass
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }

    [JsonPropertyName("stackTrace")] public string StackTrace { get; set; }

    [JsonPropertyName("innerException")] public string InnerException { get; set; }
}

public partial class Properties
{
    [JsonPropertyName("additionalProp1")] public string AdditionalProp1 { get; set; }

    [JsonPropertyName("additionalProp2")] public string AdditionalProp2 { get; set; }

    [JsonPropertyName("additionalProp3")] public string AdditionalProp3 { get; set; }
}

public partial class WorkflowState
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("definitionId")] public string DefinitionId { get; set; }

    [JsonPropertyName("definitionVersion")]
    public long DefinitionVersion { get; set; }

    [JsonPropertyName("correlationId")] public string CorrelationId { get; set; }

    [JsonPropertyName("status")] public long Status { get; set; }

    [JsonPropertyName("subStatus")] public long SubStatus { get; set; }

    [JsonPropertyName("bookmarks")] public List<Bookmark> Bookmarks { get; set; }

    [JsonPropertyName("fault")] public Fault Fault { get; set; }

    [JsonPropertyName("completionCallbacks")]
    public List<CompletionCallback> CompletionCallbacks { get; set; }

    [JsonPropertyName("activityExecutionContexts")]
    public List<ActivityExecutionContext> ActivityExecutionContexts { get; set; }

    [JsonPropertyName("output")] public Properties Output { get; set; }

    [JsonPropertyName("properties")] public Properties Properties { get; set; }
}

public partial class ActivityExecutionContext
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("parentContextId")] public string ParentContextId { get; set; }

    [JsonPropertyName("scheduledActivityNodeId")]
    public string ScheduledActivityNodeId { get; set; }

    [JsonPropertyName("ownerActivityNodeId")]
    public string OwnerActivityNodeId { get; set; }

    [JsonPropertyName("properties")] public Properties Properties { get; set; }

    [JsonPropertyName("activityState")] public Properties ActivityState { get; set; }
}

public partial class Bookmark
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("hash")] public string Hash { get; set; }

    [JsonPropertyName("payload")] public string Payload { get; set; }

    [JsonPropertyName("activityNodeId")] public string ActivityNodeId { get; set; }

    [JsonPropertyName("activityInstanceId")]
    public string ActivityInstanceId { get; set; }

    [JsonPropertyName("autoBurn")] public bool AutoBurn { get; set; }

    [JsonPropertyName("callbackMethodName")]
    public string CallbackMethodName { get; set; }
}

public partial class CompletionCallback
{
    [JsonPropertyName("ownerInstanceId")] public string OwnerInstanceId { get; set; }

    [JsonPropertyName("childNodeId")] public string ChildNodeId { get; set; }

    [JsonPropertyName("methodName")] public string MethodName { get; set; }
}