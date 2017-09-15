using System;

namespace Serilog.Sinks.EventGrid
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class EventGridTypeAttribute : Attribute, IEventGridAttribute
    {
      public string CustomValue { get; }

      public EventGridTypeAttribute(string customValue)
      {
        CustomValue = customValue;
      }

      public EventGridTypeAttribute() { }
    }
}
