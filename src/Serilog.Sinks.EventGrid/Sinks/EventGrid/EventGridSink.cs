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

    public EventGridSink(IFormatProvider formatProvider, string key, Uri topicUri)
    {
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      _key = key;
      _topicUri = topicUri;
    }

    public void Emit(LogEvent logEvent)
    {
      var customAttribute = CreateCustomEvent(logEvent.Properties);
      
      //logEvent.Properties.ContainsKey("subject")

      throw new System.NotImplementedException();
    }

    static CustomEventGridEvent CreateCustomEvent(IReadOnlyDictionary<string, LogEventPropertyValue> props)
    {
      // walk up the stack and check for custom attribute tags
      var st = new StackTrace();
      var stackFrames = st.GetFrames();
      if (stackFrames == null) return null;

      EventGridSinkAttribute myCustomAttribute;
      var customEvent = new CustomEventGridEvent();

      // first look for the first method in the stack
      var methodAttribute = stackFrames.Where(f => f != null).Select(f => f.GetMethod()).Select(m => m.GetCustomAttribute(typeof(EventGridSinkAttribute))).FirstOrDefault();
      if (methodAttribute != null)
        myCustomAttribute = methodAttribute as EventGridSinkAttribute;
      else
      {
        // then look for it on the class level
        var classAttribute = stackFrames.Where(f => f != null).Select(f => f.GetMethod()).Select(m => m.ReflectedType?.GetCustomAttribute(typeof(EventGridSinkAttribute))).FirstOrDefault();
        myCustomAttribute = classAttribute as EventGridSinkAttribute;
      }

      customEvent.Subject = myCustomAttribute?.CustomEventSubject;
      customEvent.EventType = myCustomAttribute?.CustomEvenType;

      return customEvent;
    }
  }
}
