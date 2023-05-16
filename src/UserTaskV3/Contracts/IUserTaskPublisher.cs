namespace UserTaskV3.Contracts;

public interface IUserTaskPublisher
{
    Task PublishAsync(string eventName, string? correlationId = default, string? workflowInstanceId = default,
        IDictionary<string, object>? input = default, CancellationToken cancellationToken = default);


    Task DispatchAsync(string eventName, string? correlationId = default, string? workflowInstanceId = default,
        IDictionary<string, object>? input = default, CancellationToken cancellationToken = default);

}