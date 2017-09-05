using System;
using System.Collections.Generic;
using System.Linq;

namespace Serilog.Sinks.EventGrid.Sinks.EventGrid
{
  public class CustomEventContext : ICustomEvent
  {
    public string EventType { get; set; }
    public string Subject { get; set; }

    public CustomEventContext(string eventType, string subject)
    {
      EventType = eventType;
      Subject = subject;
    }

    public CustomEventContext() { }
  }
}
