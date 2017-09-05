using System.Collections.Generic;

namespace Serilog.Sinks.EventGrid.Sinks.EventGrid
{
  public interface ICustomEvent
  {
    string EventType { get; }
    string Subject { get; }
  }
}
