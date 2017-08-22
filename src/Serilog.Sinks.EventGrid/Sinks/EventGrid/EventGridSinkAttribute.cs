using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sinks.EventGrid.Sinks.EventGrid
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class EventGridSinkAttribute : Attribute
  {
    public string CustomEventSubject { get; set; }
    public string  CustomEvenType { get; set; }

    public EventGridSinkAttribute(string customEventSubject, string customEvenType)
    {
      CustomEventSubject = customEventSubject;
      CustomEvenType = customEvenType;
    }

   public EventGridSinkAttribute(string customEventSubject) : this(customEventSubject, "serilog") { }

    public EventGridSinkAttribute() : this ("logEvent") { }
  }
}
