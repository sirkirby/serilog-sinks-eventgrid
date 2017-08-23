using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.EventGrid.Sinks.EventGrid
{
  public class EventGridSink : ILogEventSink
  {
    readonly string _key;
    readonly Uri _topicUri;
    readonly string _customEventSubject;
    readonly string _customEventType;

    public EventGridSink(IFormatProvider formatProvider, string key, Uri topicUri, string customEventSubject = null, string customEventType = null)
    {
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      _key = key;
      _topicUri = topicUri;
      _customEventSubject = customEventSubject;
      _customEventType = customEventType;

      // use the calling assembly name to ensure we have a subject
      if (string.IsNullOrEmpty(_customEventSubject))
        _customEventSubject = Assembly.GetCallingAssembly().GetName().Name;

      if (string.IsNullOrEmpty(_customEventType))
        _customEventType = "serilogLogEvent";
    }

    public void Emit(LogEvent logEvent)
    {
      var customEvent = CreateCustomEvent(logEvent.Properties);

      //TODO: http post to event grid endpoint
    }

    static CustomEventGridEvent CreateCustomEvent(IReadOnlyDictionary<string, LogEventPropertyValue> props)
    {
      // walk up the stack and check for custom attribute tags
      var st = new StackTrace();
      var stackFrames = st.GetFrames();
      if (stackFrames == null) return null;

      EventGridSinkAttribute myCustomAttribute;
      var customEvent = new CustomEventGridEvent();

      // first look for the first method in the stack with the attribute
      var methodAttribute = stackFrames.Where(f => f != null).Select(f => f.GetMethod()).Select(m => m.GetCustomAttribute(typeof(EventGridSinkAttribute))).FirstOrDefault();
      if (methodAttribute != null)
        myCustomAttribute = methodAttribute as EventGridSinkAttribute;
      else
      {
        // then look for the first class with the attribute, there can be only one
        var classAttribute = stackFrames.Where(f => f != null).Select(f => f.GetMethod()).Select(m => m.ReflectedType?.GetCustomAttribute(typeof(EventGridSinkAttribute))).FirstOrDefault();
        myCustomAttribute = classAttribute as EventGridSinkAttribute;
      }

      customEvent.Subject = myCustomAttribute?.CustomEventSubject;
      customEvent.EventType = myCustomAttribute?.CustomEvenType;

      return customEvent;
    }
  }
}
