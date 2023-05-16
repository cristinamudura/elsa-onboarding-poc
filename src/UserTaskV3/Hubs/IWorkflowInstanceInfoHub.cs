namespace UserTaskV3.Hubs;

public interface IWorkflowInstanceInfoHub
{
    Task WorkflowInstanceUpdate(WorkflowInstanceInfo workflowInstanceInfo);
}