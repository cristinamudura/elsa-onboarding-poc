using Elsa.EntityFrameworkCore.Extensions;
using Elsa.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Workflows.Core.Notifications;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors.Infrastructure;
using UserTaskV3.Activities;
using UserTaskV3.Hubs;
using UserTaskV3.Notifications;
using UserTaskV3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();
var services = builder.Services;
var configuration = builder.Configuration;
var sqliteConnectionString = configuration.GetConnectionString("Sqlite")!;
var identitySection = configuration.GetSection("Identity");
var identityTokenSection = identitySection.GetSection("Tokens");

// Add Elsa services.
services
    .AddElsa(elsa => elsa
        .UseIdentity(identity =>
        {
            identity.IdentityOptions = options => identitySection.Bind(options);
            identity.TokenOptions = options => identityTokenSection.Bind(options);
            identity.UseConfigurationBasedUserProvider(options => identitySection.Bind(options));
            identity.UseConfigurationBasedApplicationProvider(options => identitySection.Bind(options));
            identity.UseConfigurationBasedRoleProvider(options => identitySection.Bind(options));
        })
        .UseDefaultAuthentication()
        .UseWorkflowManagement(management =>
        {
            management.UseEntityFrameworkCore(ef => ef.UseSqlite(sqliteConnectionString));
            management.AddActivitiesFrom<DisplayUIActivity>();
        })
        .UseWorkflowRuntime(runtime =>
        {
            runtime.UseDefaultRuntime(dr => dr.UseEntityFrameworkCore(ef => ef.UseSqlite(sqliteConnectionString)));
            runtime.UseExecutionLogRecords(e => e.UseEntityFrameworkCore(ef => ef.UseSqlite(sqliteConnectionString)));
            runtime.UseAsyncWorkflowStateExporter();
        })
        
        .UseJavaScript()
        .UseLiquid()
        .UseHttp()
        .UseWorkflowsApi()
        .AddFastEndpointsAssembly(typeof(DisplayUIActivity))
    );


//UserTaskServices
services.AddSingleton<IUserTaskPublisher, UserTaskPublisher>();

services.AddHealthChecks();
services.AddSwaggerDoc();

static bool AllowAny(IList<string> Values) => Values.Count == 0 || Values[0] == "*";
services.AddCors(cors => {
    IConfigurationSection corsPolicyConfiguration = configuration.GetSection("CorsPolicy");
    CorsPolicy corsPolicy = corsPolicyConfiguration.Get<CorsPolicy>() ?? new CorsPolicy();
    cors.AddDefaultPolicy(policy => {
        var headers = corsPolicy.Headers;
        var origins = corsPolicy.Origins;
        var methods = corsPolicy.Methods;

        if (AllowAny(headers))
            policy.AllowAnyHeader();
        else
            policy.WithHeaders(headers.ToArray());

        if (AllowAny(origins))
            policy.AllowAnyOrigin();
        else
            policy.WithOrigins(origins.ToArray());

        if (AllowAny(methods))
            policy.AllowAnyMethod();
        else
            policy.WithMethods(methods.ToArray());
    });
});
// Notifications
services.AddSignalR();
services.AddNotificationHandler<WorkflowNotifier, ActivityExecuted>();
services.AddNotificationHandler<WorkflowNotifier, ActivityExecuting>();
services.AddNotificationHandler<WorkflowNotifier, WorkflowExecuted>();

// Razor Pages.
services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

// Configure middleware pipeline.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");
app.UseCors();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapRazorPages();
app.UseSwaggerGen();

app.UseEndpoints(endpoints =>
{
    // SignalR -> Mapping to hub to endpoint 
    endpoints.MapHub<WorkflowInstanceInfoHub>("/usertask-info");
});
app.Run();