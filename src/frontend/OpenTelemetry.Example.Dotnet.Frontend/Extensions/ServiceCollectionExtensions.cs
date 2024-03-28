
using System.Reflection;
using System.Runtime.CompilerServices;
using OpenTelemetry.Example.Dotnet.Frontend.Telemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Example.Dotnet.Frontend.Extensions;

public static class ServiceCollectionExtensions
{
    private const string TELEMETRY_EXPORTER_AGENT_ADDRESS_CONNECTION_NAME = "TelemetryExporterAgentAddress";
    private const string ORDERING_SERVICE_ADDRESS = "OrderingServiceAddress";
    private const string EVENT_CATALOG_SERVICE_ADDRESS = "EventCatalogServiceAddress";

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void AddTelemetry(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder? loggingBuilder, bool addSampler = true)
    {
        AssemblyName assemblyName = Assembly.GetCallingAssembly().GetName();
        string serviceName = assemblyName.Name ?? string.Empty;
        string serviceVersion = assemblyName.Version?.ToString() ?? string.Empty;

        services.AddSingleton<IAppActivitySource>(new AppActivitySource(serviceName, serviceVersion));

        string telemetryExporterAgentAddress = configuration.GetConnectionString(TELEMETRY_EXPORTER_AGENT_ADDRESS_CONNECTION_NAME)!;

       
       services
            .AddOpenTelemetry()
            .WithTracing
            (
                options =>
                {
                    
                    if (addSampler)
                    {
                        options.SetSampler(new AlwaysOnSampler());
                    }

                   

                    options
                        .AddAspNetCoreInstrumentation()
                      
                        .AddHttpClientInstrumentation()
                        .AddSource(serviceName)
                        .ConfigureResource(builder =>
                        {
                            builder.AddService(serviceName, serviceVersion: serviceVersion);           
                        });
                        options.AddOtlpExporter(o => o.Endpoint = new Uri(telemetryExporterAgentAddress));
                  
                }
            )
            .WithMetrics
            (
                options =>
                {
                    options.AddOtlpExporter(o => o.Endpoint = new Uri(telemetryExporterAgentAddress));
                }
           );

        
        loggingBuilder?.AddOpenTelemetry(options =>
        {
            ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault();
            resourceBuilder.AddService(serviceName, serviceVersion: serviceVersion);
            options.SetResourceBuilder(resourceBuilder);

            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.IncludeFormattedMessage = true;
            options.AddOtlpExporter(o => o.Endpoint = new Uri(telemetryExporterAgentAddress));                               
        });
    }    
    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        string orderingServiceAddress = configuration.GetConnectionString(ORDERING_SERVICE_ADDRESS)!;
        string eventCatalogServiceAddress = configuration.GetConnectionString(EVENT_CATALOG_SERVICE_ADDRESS)!;

        services.AddHttpClient("eventCatalog", config => { config.BaseAddress = new Uri(eventCatalogServiceAddress); });
        services.AddHttpClient("ordering", config => { config.BaseAddress = new Uri(orderingServiceAddress); });
    }
}