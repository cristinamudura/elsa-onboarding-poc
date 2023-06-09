﻿using ElsaDrivenWebApp.Services.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ElsaDrivenWebApp.Services
{
    public class UsertaskService
    {
        private readonly HttpClient httpClient;
        public Dictionary<string, UsertaskViewModel> workflowInstancesCache = new Dictionary<string, UsertaskViewModel>();
        public List<UsertaskViewModel> TriggerCache = new List<UsertaskViewModel>();
        public bool AreTriggersLoaded = false;

        public UsertaskService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UsertaskViewModel[]> GetWorkflowsForSignal(string signal)
        {
            return await httpClient.GetFromJsonAsync<UsertaskViewModel[]>($"/v1/usertask-signals/{signal}");
        }

        public async Task<UsertaskViewModel[]> GetWorkflowsTriggersForUsertask()
        {
            var triggers = await httpClient.GetFromJsonAsync<UsertaskViewModel[]>($"/v1/usertask-signals/triggers");
            TriggerCache = triggers == null ? new List<UsertaskViewModel>() : triggers.ToList();
            AreTriggersLoaded = true;
            return TriggerCache.ToArray();
        }

        public async Task<WorkfowInstanceUsertaskViewModel[]> GetWorkflowsWaitingOnUserTask()
        {
            return await httpClient.GetFromJsonAsync<WorkfowInstanceUsertaskViewModel[]>($"/v1/usertask-signals");
        }

        public async Task<UsertaskViewModel[]> GetWorkflowsForSignals(List<string> signals)
        {
            var result = new List<UsertaskViewModel>();
            await Task.WhenAll(signals.Select(async i => result.AddRange(await GetWorkflowsForSignal(i))));
            return result.ToArray();
        }

        public async Task<WorkfowInstanceUsertaskViewModel> GetUsertasksFor(string workflowinstanceId)
        {
            var items = await httpClient.GetFromJsonAsync<WorkfowInstanceUsertaskViewModel[]>($"/v1/usertask-signals?workflowinstanceId={workflowinstanceId}");
            return items?.FirstOrDefault();
        }

        public async Task MarkAsComplete(string workflowInstanceId, string signal, JToken signalData)
        {
            var data = new MarkAsCompletedPostModel
            {
                WorkflowInstanceId = workflowInstanceId,
                Input = signalData == null ? JValue.CreateNull() : signalData
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"/v1/usertask-signals/{signal}/execute", content);
        }

        public async Task MarkAsCompleteDispatched(string workflowInstanceId, string signal, JToken signalData)
        {
            var data = new MarkAsCompletedPostModel
            {
                WorkflowInstanceId = workflowInstanceId,
                Input = signalData == null ? JValue.CreateNull() : signalData
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"/v1/usertask-signals/{signal}/dispatch", content);
        }

        public async Task<List<WorkflowInstanceDetails>> StartWorkflowWithUserTaskDispatched(string signal, JToken signalData)
        {
            var data = new MarkAsCompletedPostModel
            {
                Input = signalData == null ? JValue.CreateNull() : signalData
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"/v1/usertask-signals/{signal}/dispatch", content);
            return await result?.Content?.ReadFromJsonAsync<List<WorkflowInstanceDetails>>();
        }
    }
}
