using System;

namespace Serilog.Sinks.EventGrid
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class EventGridSubjectAttribute : Attribute
  {
    public string CustomEventSubject { get; set; }

    public EventGridSubjectAttribute(string customEventSubject)
    {
      CustomEventSubject = customEventSubject;
    }
    public EventGridSubjectAttribute() : this (null) { }
  }
}
