using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Models;

namespace UserTaskV3.Providers;

public class UserTaskEventActivityProvider : IActivityProvider
{
    public ValueTask<IEnumerable<ActivityDescriptor>> GetDescriptorsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}