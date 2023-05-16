using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;
using UserTaskV3.Activities;
using UserTaskV3.Providers;

namespace UserTaskV3.Features;

public class UserTaskFeature : FeatureBase
{
    private const string UserTaskCategoryName = "UserTask";
    
    public UserTaskFeature(IModule module) : base(module)
    {
    }
    
    public override void Configure()
    {
        Module.UseWorkflowManagement(management =>
        {
            management.AddActivitiesFrom<UserTask>();
        });
    }
    
    public override void Apply()
    {
        Services
            .AddActivityProvider<UserTaskEventActivityProvider>();
    }
}