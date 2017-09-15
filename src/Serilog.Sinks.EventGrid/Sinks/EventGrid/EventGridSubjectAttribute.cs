using System;

namespace Serilog.Sinks.EventGrid
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class EventGridSubjectAttribute : Attribute, IEventGridAttribute
  {
    public string CustomValue { get; }

    public EventGridSubjectAttribute(string customValue)
    {
      CustomValue = customValue;
    }

    public EventGridSubjectAttribute() { }
  }
}
