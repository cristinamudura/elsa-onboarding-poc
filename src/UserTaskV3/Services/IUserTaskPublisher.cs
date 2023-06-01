namespace UserTaskV3.Services;

public interface IUserTaskPublisher
{
    Task PublishAsync(string activityId, string? correlationId = default, string? workflowInstanceId = default,
        IDictionary<string, object>? input = default, CancellationToken cancellationToken = default);


    Task DispatchAsync(string activityId, string? correlationId = default, string? workflowInstanceId = default,
        IDictionary<string, object>? input = default, CancellationToken cancellationToken = default);

}