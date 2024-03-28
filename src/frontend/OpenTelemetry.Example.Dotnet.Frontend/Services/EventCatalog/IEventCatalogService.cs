
using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;

public interface IEventCatalogService 
{
    Task<IEnumerable<Event>> GetAll();

    Task<Event> GetEvent(Guid id);

}