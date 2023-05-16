using Elsa.Http;
using Elsa.Workflows.Core.Abstractions;
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Runtime.Activities;
using UserTaskV3.Activities;

namespace ElsaEngineV3.Workflows;

public class OnBoardingWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder workflow)
    {
        workflow.Root = new Sequence
        {
            Activities =
            {
                new HttpEndpoint()
                {
                    Path = new Input<string>("/test"),
                    SupportedMethods = new Input<ICollection<string>>(() => new List<string>() {"GET"})
                },
                new WriteLine("Line 1"),
                new UserTask
                {
                    EventName = new Input<string>("test")
                },
                new Event("Resume"){ Id = "Resume"},
                new WriteLine("Line 2"),
                new WriteLine("Line 3")
            }
        };
    }
}