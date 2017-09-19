using System;
using System.Configuration;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.EventGrid;

namespace Serilog
{
  public static class LoggerConfigurationEventGridExtensions
  {
    public static LoggerConfiguration EventGrid(
      this LoggerSinkConfiguration loggerConfiguration,
      string key = null, 
      string topicEndpoint = null,
      string customEventSubject = null,
      string customEventType = null,
      string customSubjectPropertyName = "EventSubject",
      string customTypePropertyName = "EventType",
      CustomEventRequestAuth customEventRequestAuth = CustomEventRequestAuth.Key,
      LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information,
      IFormatProvider formatProvider = null)
    {
      if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

      // allow null and pull from app configuration
      key = key ?? ConfigurationManager.AppSettings["EventGridTopicKey"];
      topicEndpoint = topicEndpoint ?? ConfigurationManager.AppSettings["EventGridTopicUri"];

      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      if (string.IsNullOrWhiteSpace(topicEndpoint))
        throw new ArgumentNullException("topicEndpoint");

      var topicUri = new Uri(topicEndpoint);
      if (!topicUri.IsAbsoluteUri)
        throw new ArgumentException("topicEndpoint must be an absolute uri");

      return loggerConfiguration.Sink(
        new EventGridSink(formatProvider, key, topicUri, customEventSubject, customEventType, customSubjectPropertyName, customTypePropertyName, customEventRequestAuth), restrictedToMinimumLevel);
    }
  }
}
