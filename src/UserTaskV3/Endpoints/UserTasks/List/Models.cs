namespace UserTaskV3.Endpoints.UserTasks.List;

internal class Response
{
    public string WorkflowInstanceId { get; set; }
    
    public string CorrelationId { get; set; }
    
    public string Name { get; set; }
    
    public DateTime LastExecutedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
}