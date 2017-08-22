using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.EventGrid.Sinks.EventGrid;

namespace Serilog
{
  public static class LoggerConfigurationEventGridExtensions
  {
    public static LoggerConfiguration EventGrid(
      this LoggerSinkConfiguration loggerConfiguration,
      string key, 
      string topicEndpoint,
      LogEventLevel restrictedToMinimumLevel = LogEventLevel.Error,
      IFormatProvider formatProvider = null)
    {
      if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

      if (string.IsNullOrWhiteSpace(key))
        throw new ArgumentNullException("key");

      if (string.IsNullOrWhiteSpace(topicEndpoint))
        throw new ArgumentNullException("topicEndpoint");

      var topicUri = new Uri(topicEndpoint);
      if (!topicUri.IsAbsoluteUri)
        throw new ArgumentException("topicEndpoint must be an absolute uri");


      return loggerConfiguration.Sink(
        new EventGridSink(formatProvider, key, topicUri), restrictedToMinimumLevel);
    }
  }
}
