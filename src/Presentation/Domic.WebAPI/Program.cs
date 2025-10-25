using Domic.Core.Infrastructure.Extensions;
using Domic.Core.WebAPI.Extensions;
using Domic.Infrastructure.Extensions.C;
using Domic.Infrastructure.Extensions.Q;
using Domic.WebAPI.EntryPoints.GRPCs;
using Domic.WebAPI.Frameworks.Extensions;

using Q_SQLContext = Domic.Persistence.Contexts.Q.SQLContext;
using C_SQLContext = Domic.Persistence.Contexts.C.SQLContext;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterELK();
builder.RegisterEntityFrameworkCoreQuery<Q_SQLContext>();
builder.RegisterEntityFrameworkCoreCommand<C_SQLContext, string>();
builder.RegisterCommandRepositories();
builder.RegisterQueryRepositories();
builder.RegisterCommandQueryUseCases();
builder.RegisterGrpcServer();
builder.RegisterMessageBroker();
builder.RegisterEventStreamBroker();
builder.RegisterEventsSubscriber();
builder.RegisterEventsPublisher();
builder.RegisterDistributedCaching();
builder.RegisterServices();

builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

#endregion

/*-------------------------------------------------------------------*/

WebApplication application = builder.Build();

/*-------------------------------------------------------------------*/

//Primary processing

application.Services.AutoMigration<Q_SQLContext>(context => context.Seed());
application.Services.AutoMigration<C_SQLContext>(context => context.Seed());

/*-------------------------------------------------------------------*/

#region Middleware

if (application.Environment.IsProduction())
{
    application.UseHsts();
    application.UseHttpsRedirection();
}

application.UseRouting();

application.UseObservibility();

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

public partial class Program;