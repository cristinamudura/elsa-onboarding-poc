namespace UserTaskV3.Endpoints.UserTasks.List;


internal class Request
{
    public string WorkflowInstanceId { get; set; }
}

internal class Response
{
    public string UserTaskActivityInstanceId { get; set; }
    
    public string UIDefinition { get; set; }
    
    public bool AllowPrevious { get; set; }
    
    public IDictionary<string,object> Metadata { get; set; }
    
    public string Signal { get; set; }
    
    public string ActivityId { get; set; }
}