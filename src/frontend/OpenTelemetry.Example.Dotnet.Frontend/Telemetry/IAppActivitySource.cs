using System.Diagnostics;

namespace OpenTelemetry.Example.Dotnet.Frontend.Telemetry;

public interface IAppActivitySource
{
    ActivitySource ActivitySource { get; }
}