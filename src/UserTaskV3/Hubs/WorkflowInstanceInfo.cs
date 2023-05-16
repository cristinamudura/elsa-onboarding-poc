namespace UserTaskV3.Hubs;

public class WorkflowInstanceInfo
{
    public string WorkflowInstanceId { get; internal set; }
    public string WorkflowState { get; internal set; }
    public string ActivityId { get;internal set; }
    public string? ActivityName { get; internal set; }
    public string Action { get; internal set; }
    public bool IsUsertask { get; internal set; }
    public string Description { get; internal set; }
}