using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.EventGrid
{
  public class EventGridSink : ILogEventSink
  {
    readonly string _key;
    readonly Uri _topicUri;
    readonly string _customEventSubject;
    readonly string _customEventType;
    readonly CustomEventRequestAuth _customEventRequestAuth;

    public EventGridSink(IFormatProvider formatProvider, string key, Uri topicUri, string customEventSubject = null, string customEventType = null, CustomEventRequestAuth customEventRequestAuth = CustomEventRequestAuth.Key)
    {
      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      _key = key;
      _topicUri = topicUri;
      _customEventSubject = customEventSubject;
      _customEventType = customEventType;
      _customEventRequestAuth = customEventRequestAuth;

      if (string.IsNullOrEmpty(_customEventType))
        _customEventType = "serilogLogEvent";
    }

    public void Emit(LogEvent logEvent)
    {
      var customEvent = BuildCustomEvent();

      // add the even payload from even properties
      customEvent.Data = logEvent.Properties
        .Select(pv => new {Name = pv.Key, Value = EventGridPropertyFormatter.Simplify(pv.Value)})
        .ToDictionary(a => a.Name, b => b.Value);

      var client = new HttpClient();
      var request = new HttpRequestMessage(HttpMethod.Post, _topicUri);
      request.Headers.Add(_customEventRequestAuth == CustomEventRequestAuth.Key ? "aeg-sas-key" : "aeg-sas-token", _key);
      var body = new[] { customEvent };

      var json = JsonConvert.SerializeObject(body, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });

      request.Content = new StringContent(json);
      var response = client.SendAsync(request).Result;
    }

    CustomEventRequest BuildCustomEvent()
    {
      // walk up the stack and check for custom attribute tags
      var st = new StackTrace();
      var stackFrames = st.GetFrames();
      if (stackFrames == null) return null;
      var methods = stackFrames.Where(f => f != null).Select(f => f.GetMethod()).ToArray();
      // walk the sink back up to the calling method
      var callingMethod = methods.FirstOrDefault(m => !m?.DeclaringType?.FullName?.StartsWith("Serilog") ?? false) ?? methods.First();

      EventGridSinkAttribute myCustomAttribute;
      var customEvent = new CustomEventRequest();

      // first look for the first method in the stack with the attribute
      var methodAttribute = methods.FirstOrDefault(m => m.GetCustomAttribute<EventGridSinkAttribute>() != null)?.GetCustomAttribute<EventGridSinkAttribute>();
      if (methodAttribute != null)
        myCustomAttribute = methodAttribute;
      else
      {
        // then look for the first class with the attribute, there can be only one
        var classAttribute = methods.FirstOrDefault(m => m.ReflectedType != null && m.ReflectedType.GetCustomAttribute<EventGridSinkAttribute>() != null)?.ReflectedType?.GetCustomAttribute<EventGridSinkAttribute>();
        myCustomAttribute = classAttribute;
      }

      // assign the attribute values or fall back to the instance values
      customEvent.Subject = myCustomAttribute?.CustomEventSubject ?? _customEventSubject ?? GetSubject();
      customEvent.EventType = myCustomAttribute?.CustomEvenType ?? _customEventType;

      return customEvent;

      string GetSubject()
      {
        // try to find the method where the log event initiated from
        var assemblyName = callingMethod?.ReflectedType?.Assembly.GetName().Name;
        var className = callingMethod?.ReflectedType?.Name;
        var methodName = callingMethod?.Name;
        return $"{assemblyName ?? "UnknownApplication"}/{className}/{methodName}";

      }
    }
  }
}
