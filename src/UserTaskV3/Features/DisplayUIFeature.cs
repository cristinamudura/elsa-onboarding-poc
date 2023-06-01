// using Elsa.Extensions;
// using Elsa.Features.Abstractions;
// using Elsa.Features.Services;
// using UserTaskV3.Activities;
//
// namespace UserTaskV3.Features;
//
// public class DisplayUIFeature : FeatureBase
// {
//     private const string UserTaskCategoryName = "UserTask";
//     
//     public DisplayUIFeature(IModule module) : base(module)
//     {
//     }
//     
//     public override void Configure()
//     {
//         Module.UseWorkflowManagement(management =>
//         {
//             management.AddActivitiesFrom<DisplayUIActivity>();
//         });
//     }
//     
//     public override void Apply()
//     {
//        
//     }
// }