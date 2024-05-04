using System.Diagnostics.CodeAnalysis;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace WebinarAPI.Telemetry.Filters;

[ExcludeFromCodeCoverage]
public class DependencyTelemetryFilter(ITelemetryProcessor next) : ITelemetryProcessor
{
    public void Process(ITelemetry item)
    {
        if (item is DependencyTelemetry)
        {
            return;
        }

        next.Process(item);
    }
}