using System.Diagnostics;

namespace OpenTelemetry.Example.Dotnet.Frontend.Telemetry;

internal class AppActivitySource : IAppActivitySource, IDisposable
{
    public AppActivitySource(string serviceName, string serviceVersion)
    {
        ActivitySource = new ActivitySource(serviceName, serviceVersion);
    }

    public ActivitySource ActivitySource { get; }


    public void Dispose()
    {
        ActivitySource.Dispose();
    }
}