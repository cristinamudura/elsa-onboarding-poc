namespace UserTaskV3.Bookmarks;


public class IncomingUserTaskBookmarkPayload
{
    private readonly string _eventName = default!;

    public IncomingUserTaskBookmarkPayload(string eventName)
    {
        EventName = eventName;
    }

    public string EventName
    {
        get => _eventName;
        init => _eventName = value.ToLowerInvariant();
    }
}