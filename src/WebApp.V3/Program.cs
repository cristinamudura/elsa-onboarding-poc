using Microsoft.AspNetCore.SignalR.Client;
using WebApp.V3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
var configuration = builder.Configuration;
var services = builder.Services;


var baseAddress = configuration["UserTaskService:BaseAddress"];


//signalR for notifications 
services.AddSingleton<HubConnection>(sp => {
    return new HubConnectionBuilder()
        .WithUrl($"{baseAddress}/usertask-info")
        .WithAutomaticReconnect()
        .Build();
});

services.AddHttpClient("WorkflowDefinitionServiceClient",
    client =>
    {
        // Set the base address of the named client.
        client.BaseAddress = new Uri(baseAddress);
    });

services.AddHttpClient("AuthorizationServiceClient",
    client =>
    {
        // Set the base address of the named client.
        client.BaseAddress = new Uri(baseAddress);
    });

services.AddHttpClient("UserTaskService",
    client =>
    {
        // Set the base address of the named client.
        client.BaseAddress = new Uri(baseAddress);
    });

services.AddTransient<IAuthorizationService, AuthorizationService>();
services.AddTransient<IWorkflowDefinitionService, WorkflowDefinitionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();