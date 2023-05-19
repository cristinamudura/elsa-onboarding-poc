namespace UserTaskV3.Bookmarks;


public class IncomingUserTaskBookmarkPayload
{
    private readonly string _eventName = default!;

    private readonly string _userTaskType = "CustomUserTask";
    
    private readonly string _activityId = default!;


    public IncomingUserTaskBookmarkPayload(string eventName, string activityId)
    {
        EventName = eventName;
        ActivityId = activityId;
    }

    public string EventName
    {
        get => _eventName;
        init => _eventName = value?.ToLowerInvariant()??string.Empty;
    }

    public string ActivityId
    {
        get => _activityId;
        init => _activityId = value;
    }
    
    public string UserTaskType
    {
        get => _userTaskType;
        init => _userTaskType = value?.ToLowerInvariant() ?? string.Empty;
    }
}