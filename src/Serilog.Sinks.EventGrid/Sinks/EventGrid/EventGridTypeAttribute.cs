using System;

namespace Serilog.Sinks.EventGrid
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class EventGridTypeAttribute : Attribute
    {
      public string CustomEventType { get; set; }

      public EventGridTypeAttribute(string customEventType)
      {
        CustomEventType = customEventType;
      }
      public EventGridTypeAttribute() : this (null) { }
    }
}
