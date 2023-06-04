namespace WebApp.V3.Services;

public class UserTaskViewModel
{
    public string UserTaskActivityInstanceId { get; set; }
    
    public string UIDefinition { get; set; }
    
    public bool AllowPrevious { get; set; }
    
    public IDictionary<string,object> Metadata { get; set; }
    
    public string ActivityId { get; set; }
}