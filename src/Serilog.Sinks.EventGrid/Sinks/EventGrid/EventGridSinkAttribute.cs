using System;

namespace Serilog.Sinks.EventGrid
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

   public EventGridSinkAttribute(string customEventType) : this(null, customEventType) { }


    public EventGridSinkAttribute() : this (null) { }
  }
}
