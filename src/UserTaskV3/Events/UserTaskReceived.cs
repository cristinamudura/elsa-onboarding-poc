using Elsa.Mediator.Contracts;
using UserTaskV3.Models;

namespace UserTaskV3.Events;

public class UserTaskReceived : INotification
{
    public UserTaskReceived(UserTaskModel webhook)
    {
        Webhook = webhook;
    }
        
    public UserTaskModel Webhook { get; }
}