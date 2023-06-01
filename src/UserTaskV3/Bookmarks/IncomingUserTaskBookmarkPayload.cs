namespace UserTaskV3.Bookmarks;


public class IncomingUserTaskBookmarkPayload
{
    private readonly string _activityId = default!;


    public IncomingUserTaskBookmarkPayload(string activityId)
    {
        ActivityId = activityId;
    }

    public string ActivityId
    {
        get => _activityId;
        init => _activityId = value;
    }
}