using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;

namespace OpenTelemetry.Example.Dotnet.Frontend.Services.EventCatalog;

public class InMemoryEventCatalogService:IEventCatalogService
{

    private IEnumerable<Event> MockEvents()
    {
        var abbaGuid= Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA317}");
        var metallicaGuid = Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA318}");
        var acDcGuid = Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA319}");

        
        
        yield return new Event
        {
            EventId = abbaGuid, 
            Artist = "ABBA", 
            Date = DateTime.Now.AddDays(5), 
            Description = "ABB concert",
            Name = "ABBA Concert", 
            Price = 100
        };
        
        yield return new Event
        {
            EventId = metallicaGuid , 
            Artist = "Metallica", 
            Date = DateTime.Now.AddDays(13), 
            Description = "Battery",
            Name = "Reload", 
            Price = 300
        };
        yield return new Event
        {
            EventId = acDcGuid, 
            Artist = "AC/DC", 
            Date = DateTime.Now.AddDays(24), 
            Description = "Thunder Concert",
            Name = "Thunder", 
            Price = 150
        };
        
    }

    public Task<IEnumerable<Event>> GetAll()
    {
        return Task.FromResult(MockEvents());
    }

    public Task<Event> GetEvent(Guid id)
    {
        return Task.FromResult(MockEvents()?.Where(a => a.EventId == id).SingleOrDefault());
    }
}
