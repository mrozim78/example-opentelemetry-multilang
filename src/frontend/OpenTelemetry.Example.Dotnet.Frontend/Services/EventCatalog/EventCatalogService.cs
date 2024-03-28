using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using OpenTelemetry.Example.Dotnet.Frontend.Extensions;
using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;
using OpenTelemetry.Example.Dotnet.Frontend.Telemetry;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;

public class EventCatalogService:IEventCatalogService
{
    private IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;
    private readonly ILogger<EventCatalogService> _logger;
    private IAppActivitySource _activitySource;
    public EventCatalogService(IHttpClientFactory httpClientFactory , ILogger<EventCatalogService> logger, IAppActivitySource activitySource)
    {
        _httpClientFactory = httpClientFactory;
        _client = _httpClientFactory.CreateClient("eventCatalog");
        _logger = logger;
        _activitySource = activitySource;
    }

    public async Task<IEnumerable<Event>> GetAll()
    {
        var response = await _client.GetAsync("event");

        using Activity? activity = _activitySource.ActivitySource.StartActivity(ActivityKind.Client);
        activity?.AddEvent(new("Start get events"));
        List<Event> events = await response.ReadContentAs<List<Event>>();
        activity?.AddEvent(new("End get events"));
        return events;
    }

    public async Task<Event> GetEvent(Guid id)
    {
        using Activity? activity = _activitySource.ActivitySource.StartActivity(ActivityKind.Client);
        activity?.AddEvent(new("Start get event"));
        var response = await _client.GetAsync($"event/{id}");
        activity?.AddEvent(new("End get event"));
        return await response.ReadContentAs<Event>();
    }

}