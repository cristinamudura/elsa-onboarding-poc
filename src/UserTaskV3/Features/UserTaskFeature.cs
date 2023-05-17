using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using UserTaskV3.Activities;

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
       
    }
}