using OpenTelemetry.Example.Dotnet.Frontend.Models.Api;

namespace OpenTelemetry.Example.Dotnet.Frontend.Models.View;

public class EventListModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public IEnumerable<Event> Events { get; set; } = new List<Event>();
    public int NumberOfItems { get; set; }
}