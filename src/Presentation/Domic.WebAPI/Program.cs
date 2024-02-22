using Domic.Core.Infrastructure.Extensions;
using Domic.Core.WebAPI.Extensions;
using Domic.Infrastructure.Extensions.Q;
using Domic.Persistence.Contexts.Q;
using Domic.WebAPI.EntryPoints.GRPCs;
using Domic.WebAPI.Frameworks.Extensions;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterELK();
builder.RegisterEntityFrameworkCoreQuery<SQLContext>();
builder.RegisterQueryRepositories();
builder.RegisterCommandQueryUseCases();
builder.RegisterGrpcServer();
builder.RegisterMessageBroker();
builder.RegisterEventsSubscriber();t
builder.RegisterServices();

builder.Services.AddMvc();

#endregion

/*-------------------------------------------------------------------*/

WebApplication application = builder.Build();

/*-------------------------------------------------------------------*/

//Primary processing

application.Services.AutoMigration<SQLContext>(context => context.Seed());

/*-------------------------------------------------------------------*/

#region Middleware

if (application.Environment.IsProduction())
{
    application.UseHsts();
    application.UseHttpsRedirection();
}

application.UseRouting();

application.UseEndpoints(endpoints => {
    
    endpoints.HealthCheck(application.Services);

    #region GRPC's Services

    endpoints.MapGrpcService<AuthRPC>();

    #endregion

});

#endregion

/*-------------------------------------------------------------------*/

application.Run();

/*-------------------------------------------------------------------*/

//For Integration Test

public partial class Program {}